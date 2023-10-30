using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration;

public class VendorOrderDetailEntityConfiguration: IEntityTypeConfiguration<VendorOrderDetail>
{
    public void Configure(EntityTypeBuilder<VendorOrderDetail> builder)
    {
        builder.ConfigurationBase<long, string, VendorOrderDetail>();

        builder.Property(x => x.QuantityOnHand).HasColumnType("numeric(15,2)");
        builder.Property(x => x.OrderAmount).HasColumnType("numeric(15,2)");
        builder.HasOne(x => x.VendorOrder)
            .WithMany(x => x.VendorOrderDetails)
            .HasForeignKey( x => x.VendorOrderId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Product)
            .WithMany(x => x.VendorOrderDetails)
            .HasForeignKey( x => x.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}