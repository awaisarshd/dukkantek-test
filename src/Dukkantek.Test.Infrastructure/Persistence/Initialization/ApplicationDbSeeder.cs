using Bogus;
using Dukkantek.Test.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dukkantek.Test.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedProductStatus(dbContext);
        await SeedProductCategories(dbContext);
        await SeedProducts(dbContext);
    }

    private static async Task SeedProductStatus(ApplicationDbContext dbContext)
    {
        var statuses = new List<ProductStatus>()
        {
            new ProductStatus(){Name ="Sold", Active=true},
            new ProductStatus(){Name ="In Stock", Active=true},
            new ProductStatus(){Name ="Damaged", Active=true}
        };

        foreach (var status in statuses)
        {
            if (!await dbContext.ProductStatuses.AnyAsync(x => x.Name.Equals(status.Name)))
            {
                await dbContext.AddAsync(status);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedProductCategories(ApplicationDbContext dbContext)
    {
        var categories = new List<ProductCategory>()
        {
            new ProductCategory(){Name ="Books", Active=true},
            new ProductCategory(){Name ="Electronics", Active=true},
            new ProductCategory(){Name ="Sports", Active=true},
            new ProductCategory(){Name ="Mobiles", Active=true},
            new ProductCategory(){Name ="Appliances", Active=true},
            new ProductCategory(){Name ="Automotive", Active=true},
            new ProductCategory(){Name ="Home", Active=true},
            new ProductCategory(){Name ="Tools", Active=true}
        };

        foreach (var status in categories)
        {
            if (!await dbContext.ProductCategories.AnyAsync(x => x.Name.Equals(status.Name)))
            {
                await dbContext.AddAsync(status);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedProducts(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Products.AnyAsync())
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Barcode, f => f.Commerce.Ean13())
                .RuleFor(p => p.Decription, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Weighted, f => f.Random.Bool())
                .RuleFor(p => p.CategoryId, f => f.Random.Number(1, 8))
                .RuleFor(p => p.StatusId, f => f.Random.Short(1,3));

            var products = productFaker.Generate(100);

            await dbContext.Products.AddRangeAsync(products);

            await dbContext.SaveChangesAsync();
        }
    }
}
