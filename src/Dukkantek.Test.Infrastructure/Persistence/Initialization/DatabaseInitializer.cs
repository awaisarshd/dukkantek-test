using Dukkantek.Test.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dukkantek.Test.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApplicationDbSeeder _dbSeeder;

    public DatabaseInitializer(ApplicationDbContext dbContext, ApplicationDbSeeder dbSeeder, ILogger<DatabaseInitializer> logger)
    {
        _dbContext = dbContext;
        _dbSeeder = dbSeeder;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
            }
        }
    }
}