using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class VendorProductEntityConfiguration : IEntityTypeConfiguration<VendorProduct>
    {
        public void Configure(EntityTypeBuilder<VendorProduct> builder)
        {
            builder.ConfigurationBase<long, string, VendorProduct>();

            builder.Property(x => x.IsPrimary).HasDefaultValue(false);
            builder.Property(x => x.ProductCode).IsRequired();

            builder.Property(x => x.UnitCost).IsRequired().HasColumnType("numeric(18,2)");
            builder.Property(x => x.CaseCost).IsRequired().HasColumnType("numeric(18,2)");

            builder.HasOne(x => x.Product).WithMany(x => x.VendorProducts).HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Vendor).WithMany(x => x.VendorProducts).HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}