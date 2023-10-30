using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    
    public class EndOfDayEntityConfiguration : IEntityTypeConfiguration<EndOfDay>
    {
        public void Configure(EntityTypeBuilder<EndOfDay> builder)
        {
            builder.ConfigurationBase<long, string, EndOfDay>();

            builder.Property(x => x.CashTotalCounted).HasColumnType("numeric(18,2)");
            builder.Property(x => x.CashToDeposit).HasColumnType("numeric(18,2)");
            builder.Property(x => x.TenderedCashTotal).HasColumnType("numeric(18,2)");
            builder.Property(x => x.CashOverShort).HasColumnType("numeric(18,2)");
            builder.Property(x => x.TotalNotTaxableSales).HasColumnType("numeric(18,2)");
            builder.Property(x => x.TotalTaxableSales).HasColumnType("numeric(18,2)");
            builder.Property(x => x.SalesTaxCollected).HasColumnType("numeric(18,2)");
            builder.Property(x => x.TotalSales).HasColumnType("numeric(18,2)");
            builder.Property(x => x.TotalFeeAndCharges).HasColumnType("numeric(18,2)");
            builder.Property(x => x.BottleReturnTotal).HasColumnType("numeric(18,2)");
            builder.Property(x => x.RefundReturn).HasColumnType("numeric(18,2)");
            builder.Property(x => x.RefundReturnOther).HasColumnType("numeric(18,2)");
            builder.Property(x => x.RefundReturnEbt).HasColumnType("numeric(18,2)");
            builder.Property(x => x.DebitCashBack).HasColumnType("numeric(18,2)");
            builder.Property(x => x.EBTCashBack).HasColumnType("numeric(18,2)");
            builder.Property(x => x.TotalCash).HasColumnType("numeric(18,2)");
            builder.Property(x => x.CreditCardSales).HasColumnType("numeric(18,2)");
            builder.Property(x => x.SnapEBTSales).HasColumnType("numeric(18,2)");
            builder.Property(x => x.PaidOut).HasColumnType("numeric(18,2)");

            builder.HasMany(x => x.Sales).WithOne(x => x.EndOfDay).HasForeignKey(x => x.EndOfDayId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Taxes).WithOne(x => x.EndOfDay).HasForeignKey(x => x.EndOfDayId).OnDelete(DeleteBehavior.Cascade);

        }
    }
    
    public class SaleTaxResumeEntityConfiguration : IEntityTypeConfiguration<SaleTaxResume>
    {
        public void Configure(EntityTypeBuilder<SaleTaxResume> builder)
        {
            builder.ConfigurationBase<long, string, SaleTaxResume>();

            builder.Property(x => x.Amount).HasColumnType("numeric(18,2)");
        }
    }
}