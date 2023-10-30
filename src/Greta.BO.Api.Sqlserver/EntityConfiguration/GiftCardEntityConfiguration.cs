using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class GiftCardEntityConfiguration: IEntityTypeConfiguration<GiftCard>
    {
        public void Configure(EntityTypeBuilder<GiftCard> builder)
        {
            builder.ConfigurationBase<long, string, GiftCard>(userRequired: false);

            builder.Property(x => x.Number).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(p => p.Number).IsUnique();
            builder.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            builder.Property(x => x.Balance).HasColumnType("numeric(18,2)");

            builder.HasOne(x => x.Device).WithMany(x=>x.GiftCards).HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Store).WithMany(x=>x.GiftCards).HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class GiftCardTransactionEntityConfiguration: IEntityTypeConfiguration<GiftCardTransaction>
    {
        public void Configure(EntityTypeBuilder<GiftCardTransaction> builder)
        {
            builder.ConfigurationBase<long, string, GiftCardTransaction>(userRequired: false);

            builder.Property(x => x.Amount).HasColumnType("numeric(18,2)");

            builder.HasOne(x => x.GiftCard).WithMany(x=>x.Transactions).HasForeignKey(x => x.GiftCardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}