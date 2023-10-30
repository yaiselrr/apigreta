using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class StoreEntityConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ConfigurationBase<long, string, Store>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.GuidId).IsRequired();
            builder.HasIndex(p => p.GuidId).IsUnique();

            builder.Property(x => x.CashDiscountValue).HasColumnType("numeric(18,2)");
            
            builder.Property(x => x.CreditCardNeedSignatureAmount).HasColumnType("numeric(18,2)");
            builder.Property(x => x.DebitCardCashBackMaxAmount).HasColumnType("numeric(18,2)");
            builder.Property(x => x.SnapEBTCAshCashBackMaxAmount).HasColumnType("numeric(18,2)");
            builder.Property(x => x.DefaulBottleDeposit).HasColumnType("numeric(18,2)");
            builder.Property(x => x.CreditCardCalculation).HasColumnType("numeric(18,2)");
            builder.HasMany(x => x.Taxs);

            builder.HasMany(x => x.Synchros)
                .WithOne()
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Region)
                .WithMany()
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}