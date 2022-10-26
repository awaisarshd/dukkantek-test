using Dukkantek.Test.Application.Common.Interfaces;

using Dukkantek.Test.Infrastructure.Common;
using Dukkantek.Test.Infrastructure.Middleware;
using Dukkantek.Test.Infrastructure.Persistence;
using Dukkantek.Test.Infrastructure.Persistence.Initialization;
using Dukkantek.Test.Infrastructure.Serialization;
using Dukkantek.Test.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dukkantek.Test.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddPersistence(config)
                .AddRouting(options => options.LowercaseUrls = true);

            services.AddScoped<ExceptionMiddleware>();

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IJsonSerializer, NewtonSoftService>();

            return services;
        }

        public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeDatabasesAsync(cancellationToken);
        }

        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();

        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            services
            .AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

            services
                .AddDbContext<ApplicationDbContext>((p, m) =>
                {
                    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
                })
                .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
                .AddTransient<ApplicationDbSeeder>();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


            return services;
        }

        internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            return dbProvider.ToLower() switch
            {
                DbProviderKeys.SqlServer => builder.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
            };
        }

    }
}
