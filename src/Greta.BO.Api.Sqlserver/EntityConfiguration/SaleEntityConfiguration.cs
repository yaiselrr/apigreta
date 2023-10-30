using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class SaleEntityConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ConfigurationBase<long, string, Sale>(userRequired: false);

            builder.HasIndex(p => p.Invoice);

            builder.Property(x => x.SubTotal).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Discount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Tax).HasColumnType("numeric(18,3)");
            builder.Property(x => x.ServiceFee).HasColumnType("numeric(18,3)");
            builder.Property(x => x.CashDiscount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.TenderCash).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Total).HasColumnType("numeric(18,3)");
            builder.Property(x => x.ChangeDue).HasColumnType("numeric(18,3)");
            builder.Property(x => x.ClearCashDiscountTotal).HasColumnType("numeric(18,3)");
            builder.Property(x => x.CustomerDiscountAmount).HasColumnType("numeric(18,3)");
        }
    }

    // public class SaleDriverLicenseEntityConfiguration : IEntityTypeConfiguration<SaleDriverLicense>
    // {
    //     public void Configure(EntityTypeBuilder<SaleDriverLicense> builder)
    //     {
    //         builder.ConfigurationBase<long, string, SaleDriverLicense>(userRequired: false);
    //         builder.HasOne(x => x.Sale).WithOne(x => x.DriverLicense).HasForeignKey<Sale>(x => x.DriverLicenseId).OnDelete(DeleteBehavior.Cascade);
    //     }
    // }

    public class SaleTenderEntityConfiguration : IEntityTypeConfiguration<SaleTender>
    {
        public void Configure(EntityTypeBuilder<SaleTender> builder)
        {
            builder.ConfigurationBase<long, string, SaleTender>(userRequired: false);

            builder.Property(x => x.Amount).HasColumnType("numeric(18,3)");

            builder.Property(x => x.RequestedAmount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.ApprovedAmount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.RemainingBalance).HasColumnType("numeric(18,3)");
            builder.Property(x => x.ExtraBalance).HasColumnType("numeric(18,3)");
            builder.Property(x => x.CashBack).HasColumnType("numeric(18,3)");


            builder.HasOne(x => x.Sale).WithMany(x => x.Tenders).HasForeignKey(x => x.SaleId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SaleTaxEntityConfiguration : IEntityTypeConfiguration<SaleTax>
    {
        public void Configure(EntityTypeBuilder<SaleTax> builder)
        {
            builder.ConfigurationBase<long, string, SaleTax>(userRequired: false);

            builder.Property(x => x.Amount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Value).HasColumnType("numeric(18,3)");
            builder.HasOne(x => x.Sale).WithMany(x => x.Taxs).HasForeignKey(x => x.SaleId).OnDelete(DeleteBehavior.Cascade);
            //builder.HasOne(x => x.SaleProduct).WithMany(x => x.Taxs).HasForeignKey(x => x.SaleProductId).OnDelete(DeleteBehavior.NoAction);
        }
    }
    
    public class SaleDiscountEntityConfiguration : IEntityTypeConfiguration<SaleDiscount>
    {
        public void Configure(EntityTypeBuilder<SaleDiscount> builder)
        {
            builder.ConfigurationBase<long, string, SaleDiscount>(userRequired: false);

            builder.Property(x => x.Amount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Value).HasColumnType("numeric(18,3)");
            builder.HasOne(x => x.Sale).WithMany(x => x.Discounts).HasForeignKey(x => x.SaleId).OnDelete(DeleteBehavior.Cascade);
        }
    }
    
    public class SaleProductRemovedEntityConfiguration : IEntityTypeConfiguration<SaleProductRemoved>
    {
        public void Configure(EntityTypeBuilder<SaleProductRemoved> builder)
        {
            builder.ConfigurationBase<long, string, SaleProductRemoved>(userRequired: false);

            builder.Property(x => x.Price).HasColumnType("numeric(18,3)");
            builder.Property(x => x.TotalPrice).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Quantity).HasColumnType("numeric(18,3)");

            builder.HasOne(x => x.Sale).WithMany(x => x.SaleProductRemoveds).HasForeignKey(x => x.SaleId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SaleFeeEntityConfiguration : IEntityTypeConfiguration<SaleFee>
    {
        public void Configure(EntityTypeBuilder<SaleFee> builder)
        {
            builder.ConfigurationBase<long, string, SaleFee>(userRequired: false);

            builder.Property(x => x.Amount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Value).HasColumnType("numeric(18,3)");
            builder.HasOne(x => x.Sale).WithMany(x => x.Fees).HasForeignKey(x => x.SaleId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SaleProductEntityConfiguration : IEntityTypeConfiguration<SaleProduct>
    {
        public void Configure(EntityTypeBuilder<SaleProduct> builder)
        {
            builder.ConfigurationBase<long, string, SaleProduct>(userRequired: false);

            builder.Property(x => x.Price).HasColumnType("numeric(18,3)");
            builder.Property(x => x.Cost).HasColumnType("numeric(18,3)");
            builder.Property(x => x.GrossProfit).HasColumnType("numeric(18,3)");
            builder.Property(x => x.NetWeigth).HasColumnType("numeric(18,3)");
            builder.Property(x => x.MixMatchDiscount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.MixAndMatchGlobalDiscount).HasColumnType("numeric(18,3)");
            builder.Property(x => x.QTY).HasColumnType("numeric(18,3)");
            //builder.Property(x => x.DiscountValue).HasColumnType("numeric(18,3)");
            builder.Property(x => x.TaxValue).HasColumnType("numeric(18,3)");
            builder.Property(x => x.TotalPrice).HasColumnType("numeric(18,3)");
            builder.Property(x => x.CleanTotalPrice).HasColumnType("numeric(18,3)");
            builder.Property(x => x.DiscountValue).HasColumnType("numeric(18,3)");

            builder.HasOne(x => x.Sale).WithMany(x => x.Products).HasForeignKey(x => x.SaleId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}


