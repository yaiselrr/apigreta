using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class StoreProductEntityConfiguration : IEntityTypeConfiguration<StoreProduct>
    {
        public void Configure(EntityTypeBuilder<StoreProduct> builder)
        {
            builder.ConfigurationBase<long, string, StoreProduct>();

            builder.HasIndex(x => new {x.ProductId, x.StoreId}).IsUnique();

            builder.Property(x => x.Price).HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(x => x.Price2).HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(x => x.WebPrice).HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(x => x.Cost).HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(x => x.GrossProfit).HasColumnType("numeric(18,2)");
            builder.Property(x => x.GrossProfit2).HasColumnType("numeric(18,2)");
            builder.Property(x => x.WebGrossProfit).HasColumnType("numeric(18,2)");
            builder.Property(x => x.TargetGrossProfit).HasColumnType("numeric(18,2)");
            
            builder.Property(x => x.QtyHand).HasColumnType("numeric(18,2)");
            builder.Property(x => x.OrderTrigger).HasColumnType("numeric(18,2)");
            builder.Property(x => x.OrderAmount).HasColumnType("numeric(18,2)");
            
            builder.Property(x => x.SplitCount).HasColumnType("numeric(18,2)");
            
            builder.HasOne(x => x.BinLocation).WithMany(x => x.StoreProducts).HasForeignKey(sp => sp.BinLocationId).OnDelete(DeleteBehavior.NoAction);
            
            
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(sp => sp.StoreId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Product).WithMany(e => e.StoreProducts).HasForeignKey(sp => sp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(x => x.Parent).WithOne(x => x.Child).HasForeignKey<StoreProduct>(sp => sp.ParentId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Taxs);
            // builder.HasMany(x => x.Discounts);
        }
    }
}