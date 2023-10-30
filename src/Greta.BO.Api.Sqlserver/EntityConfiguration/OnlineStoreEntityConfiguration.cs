using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class OnlineStoreEntityConfiguration : IEntityTypeConfiguration<OnlineStore>
    {
        public void Configure(EntityTypeBuilder<OnlineStore> builder)
        {
            builder.ConfigurationBase<long, string, OnlineStore>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.NameWebsite).HasColumnType("varchar(100)");
            builder.HasIndex(x => x.NameWebsite).IsUnique();
            builder.Property(x => x.LocationServerType);

            builder.HasMany(x => x.Departments).WithMany(x => x.OnlineStores);

            builder.HasOne(x => x.Store)
                .WithMany(x => x.OnlineStores)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }

    public class OnlineCategoryEntityConfiguration : IEntityTypeConfiguration<OnlineCategory>
    {
        public void Configure(EntityTypeBuilder<OnlineCategory> builder)
        {
            builder.ConfigurationBase<long, string, OnlineCategory>();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.OnlineCategories).HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.OnlineStore)
                .WithMany(x => x.OnlineCategories).HasForeignKey(x => x.OnlineStoreId)
                .OnDelete(DeleteBehavior.Cascade);
            ;
        }
    }
    
    public class OnlineProductEntityConfiguration : IEntityTypeConfiguration<OnlineProduct>
    {
        public void Configure(EntityTypeBuilder<OnlineProduct> builder)
        {
            builder.ConfigurationBase<long, string, OnlineProduct>();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.OnlineProducts).HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.OnlineStore)
                .WithMany(x => x.OnlineProducts).HasForeignKey(x => x.OnlineStoreId)
                .OnDelete(DeleteBehavior.Cascade);
            ;
        }
    }
}