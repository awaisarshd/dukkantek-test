using Dukkantek.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dukkantek.Test.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));

            builder.Property(e => e.Name).HasColumnType("nvarchar(256)").IsRequired();
            builder.Property(e => e.Barcode).HasColumnType("nvarchar(64)");
            builder.Property(e => e.Decription).HasColumnType("nvarchar(4000)");

            builder.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Status).WithMany().HasForeignKey(e => e.StatusId).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(e => e.DomainEvents);
        }
    }

    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable(nameof(ProductCategory));

            builder.Property(e => e.Name).HasColumnType("nvarchar(64)");
        }
    }

    public class ProductStatusConfiguration : IEntityTypeConfiguration<ProductStatus>
    {
        public void Configure(EntityTypeBuilder<ProductStatus> builder)
        {
            builder.ToTable(nameof(ProductStatus));

            builder.Property(e => e.Name).HasColumnType("nvarchar(64)");
        }
    }

}
