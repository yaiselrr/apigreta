using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class FunctionGroupEntityConfiguration : IEntityTypeConfiguration<FunctionGroup>
    {
        public void Configure(EntityTypeBuilder<FunctionGroup> builder)
        {
            builder.ConfigurationBase<long, string, FunctionGroup>();

            builder.Property(x => x.Name).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();


            // builder
            //     .HasOne(x => x.Profile)
            //     .WithMany(x => x.FunctionGroups)
            //     .HasForeignKey(x => x.ProfileId)
            //     .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(x => x.ClientApplication)
                .WithMany(x => x.FunctionGroups)
                .HasForeignKey(x => x.ClientApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasMany(x => x.Permissions)
                .WithOne(x => x.FunctionGroup)
                .HasForeignKey(x => x.FunctionGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            #region Seed

            var creationDate = new DateTime(2021, 3, 10);
            builder.HasData(new List<FunctionGroup>
            {
                Build(1, 1, "Tax", creationDate),
                Build(1, 2, "Discount", creationDate),
                Build(1, 3, "Category", creationDate),
                Build(1, 4, "External Scale", creationDate),
                Build(1, 5, "Family", creationDate),
                Build(1, 6, "Profiles", creationDate),
                Build(1, 7, "Scale Brand", creationDate),
                Build(1, 8, "Scale Category", creationDate),
                Build(1, 9, "Scale label type", creationDate),
                Build(1, 10, "Store", creationDate),
                Build(1, 12, "Tender Type", creationDate),
                Build(1, 13, "Vendor", creationDate),
                Build(1, 14, "Vendor Contact", creationDate),
                //Build(1, 15, "SCOM", creationDate),
                //Build(1, 16, "Settings", creationDate),
                Build(1, 17, "User", creationDate),
                Build(1, 18, "Role", creationDate),
                Build(1, 19, "Price Batch", creationDate),
                Build(1, 20, "Ad Batch", creationDate),
                Build(1, 21, "Mix and Match", creationDate),
                Build(1, 22, "Product", creationDate),
                Build(1, 23, "Department", creationDate),
                Build(1, 24, "Shelf Tags", creationDate),
                Build(1, 25, "Price Family", creationDate),
                Build(1, 26, "Region", creationDate),
                Build(1, 27, "Report", creationDate),
                Build(1, 28, "Employee", creationDate),
                Build(1, 29, "Customer", creationDate),
                Build(1, 30, "General Setting", creationDate),
                Build(1, 31, "POS Config", creationDate),
                //Build(1, 32, "Report Settings", creationDate),
                Build(1, 33, "Home Screen", creationDate),
                Build(1, 37, "Scale Home FAV", creationDate),
                Build(1, 40, "Fees and Charges ", creationDate),
                Build(1, 41, "Device", creationDate),
                Build(1, 42, "End of Day", creationDate),
                Build(1, 43, "Bin Location", creationDate),
                Build(1, 44, "Inventory", creationDate),
                Build(1, 45, "Loyalty Discount", creationDate),
                Build(1, 46, "Vendor Order", creationDate),
                Build(1, 47, "Reason Codes", creationDate),
                Build(1, 48, "Rancher", creationDate),
                Build(1, 49, "Breed", creationDate),
                Build(1, 50, "Grind", creationDate),
                Build(1, 51, "Animal", creationDate),
                Build(1, 52, "Schedule", creationDate),
                Build(1, 53, "Scalendar", creationDate),


                //POS Functions
                Build(2, 101, "Sell", creationDate),
                Build(2, 102, "Pos Buttons", creationDate)
            });

            #endregion Seed
        }

        //Application 1 Backoffice
        //Application 2 POS
        private FunctionGroup Build(
            long application,
            long id,
            string name,
            DateTime creationDate)
        {
            return new()
            {
                Id = id,
                Name = name,
                State = true,
                ClientApplicationId = application,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = creationDate,
                UpdatedAt = creationDate
            };
        }
    }
}