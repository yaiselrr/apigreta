using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class TenderTypeEntityConfiguration : IEntityTypeConfiguration<TenderType>
    {
        public void Configure(EntityTypeBuilder<TenderType> builder)
        {
            builder.ConfigurationBase<long, string, TenderType>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(x => x.OpenDrawer).HasDefaultValue(false);
            builder.Property(x => x.DisplayAs).HasColumnType("varchar(64)").IsRequired();

            #region Seed
            var date = new DateTime(2022, 12, 31, 0, 0, 0);
            var tenderTypes = new List<TenderType>
            {
                new TenderType()
                {
                    Id= 1,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = true,
                    Name = "Cash",
                    OpenDrawer = true,
                    DisplayAs = "Cash",
                    CashDiscount = true
                },
                new TenderType()
                {
                    Id= 2,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = true,
                    Name = "Check",
                    OpenDrawer = false,
                    DisplayAs = "Check",
                    CashDiscount = false
                },
                new TenderType()
                {
                    Id= 3,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = false,
                    Name = "Credit Card",
                    OpenDrawer = false,
                    DisplayAs = "Credit Card",
                    CashDiscount = false,
                    PaymentGateway = true
                },
                new TenderType()
                {
                    Id= 4,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = false,
                    Name = "Debit Card",
                    OpenDrawer = false,
                    DisplayAs = "Debit Card",
                    CashDiscount = false,
                    PaymentGateway = true
                },
                new TenderType()
                {
                    Id= 5,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = false,
                    Name = "Snap/EBT",
                    OpenDrawer = false,
                    DisplayAs = "Snap/EBT",
                    CashDiscount = true,
                    PaymentGateway = true
                },
                new TenderType()
                {
                    Id= 6,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = false,
                    Name = "Snap/EBT Cash",
                    OpenDrawer = false,
                    DisplayAs = "Snap/EBT Cash",
                    CashDiscount = true,
                    PaymentGateway = true
                },
                new TenderType()
                {
                    Id= 7,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = false,
                    Name = "Gift Card",
                    OpenDrawer = false,
                    DisplayAs = "Gift Card",
                    CashDiscount = false,
                    PaymentGateway = true
                },
                new TenderType()
                {
                    Id= 8,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = date,
                    UpdatedAt = date,
                    State = true,
                    Name = "Cards",
                    OpenDrawer = false,
                    DisplayAs = "Cards",
                    CashDiscount = false,
                    PaymentGateway = true
                }
            };
            builder.HasData(tenderTypes);

            #endregion
        }
    }
}