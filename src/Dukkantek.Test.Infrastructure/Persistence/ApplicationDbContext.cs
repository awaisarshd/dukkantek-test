using Dukkantek.Test.Application.Common.Interfaces;

using Dukkantek.Test.Domain.Common;

using Dukkantek.Test.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using System.Reflection;

namespace Dukkantek.Test.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDomainEventService _domainEventService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventService domainEventService) : base(options)
    {
        _domainEventService = domainEventService;
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    public DbSet<ProductStatus> ProductStatuses => Set<ProductStatus>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = "System";
                    entry.Entity.Created = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = "System";
                    entry.Entity.LastModified = DateTime.Now;
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}
