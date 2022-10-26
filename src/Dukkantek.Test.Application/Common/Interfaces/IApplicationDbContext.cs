using Dukkantek.Test.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dukkantek.Test.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<ProductCategory> ProductCategories { get; }
    DbSet<ProductStatus> ProductStatuses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
