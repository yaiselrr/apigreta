using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration;

public class VendorOrderDetailCreditEntityConfiguration: IEntityTypeConfiguration<VendorOrderDetailCredit>
{
    public void Configure(EntityTypeBuilder<VendorOrderDetailCredit> builder)
    {
        builder.ConfigurationBase<long, string, VendorOrderDetailCredit>();

        builder.Property(x => x.CreditQuantity).HasColumnType("numeric(15,2)");
        builder.Property(x => x.CreditCost).HasColumnType("numeric(15,2)");
            
        builder.HasOne(x => x.VendorOrderDetail)
            .WithMany(x => x.VendorOrderDetailCredits)
            .HasForeignKey( x => x.VendorOrderDetailId)
            .OnDelete(DeleteBehavior.Cascade);
        
        /*builder.HasOne(x => x.VendorOrder)
            .WithMany(x => x.VendorOrderDetailCredits)
            .HasForeignKey( x => x.VendorOrderId)
            .OnDelete(DeleteBehavior.Cascade);*/
    }
}