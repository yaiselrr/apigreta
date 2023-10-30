using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ConfigurationBase<long, string, Permission>();

            builder.Property(x => x.Name).HasColumnType("varchar(40)").IsRequired();
            // builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(x => x.Code).HasColumnType("varchar(40)").IsRequired();

            builder
                .HasOne(x => x.FunctionGroup)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.FunctionGroupId)
                .OnDelete(DeleteBehavior.NoAction);


            #region Seed

            var creationDate = new DateTime(2021, 3, 10);

            var perms = new List<Permission>();

            #region Backoffice

            //Tax
            perms.AddRange(BuildCrudDefault(1, 1, "tax", creationDate));
            //Discount
            perms.AddRange(BuildCrudDefault(2, 4, "discount", creationDate));
            //Category"   
            perms.AddRange(BuildCrudDefault(3, 7, "category", creationDate));
            //External 
            perms.AddRange(BuildCrudDefault(4, 10, "external_scale", creationDate));
            //Family",    
            perms.AddRange(BuildCrudDefault(5, 13, "family", creationDate));
            //Profiles"     
            perms.AddRange(BuildCrudDefault(6, 16, "profiles", creationDate));
            //Scale Brand   
            perms.AddRange(BuildCrudDefault(7, 19, "scale_brand", creationDate));
            //Scale Category
            perms.AddRange(BuildCrudDefault(8, 22, "scale_category", creationDate));
            //Scale label type
            perms.AddRange(BuildCrudDefault(9, 25, "scale_label_type", creationDate));
            //"Store",    
            perms.AddRange(BuildCrudDefault(10, 28, "store", creationDate));
            //"Tender Type       
            perms.AddRange(BuildCrudDefault(12, 34, "tender_type", creationDate));
            //"Vendor",
            perms.AddRange(BuildCrudDefault(13, 37, "vendor", creationDate));
            //"Vendor",
            perms.AddRange(BuildCrudDefault(14, 40, "vendor_contact", creationDate));

            //"SCOM",
            //perms.AddRange(BuildCrudDefault(15, 43, "scom", creationDate));
            //"Settings",
            //perms.AddRange(BuildCrudDefault(16, 46, "settings", creationDate));
            //"User",
            perms.AddRange(BuildCrudDefault(17, 49, "user", creationDate));
            //"Role",
            perms.AddRange(BuildCrudDefault(18, 52, "role", creationDate));
            //"Price Batch",
            perms.AddRange(BuildCrudDefault(19, 55, "price_batch", creationDate));
            //"Ad Batch",
            perms.AddRange(BuildCrudDefault(20, 58, "ad_batch", creationDate));
            //"Mix and Match",
            perms.AddRange(BuildCrudDefault(21, 61, "mix_and_match", creationDate));
            //"Product",
            perms.AddRange(BuildCrudDefault(22, 64, "product", creationDate));
            //"Department",
            perms.AddRange(BuildCrudDefault(23, 67, "department", creationDate));
            //"Shelf Tags",
            perms.AddRange(BuildCrudDefault(24, 70, "shelf_tags", creationDate));
            //"Price Family",
            perms.AddRange(BuildCrudDefault(25, 73, "price_family", creationDate));
            //"Region",
            perms.AddRange(BuildCrudDefault(26, 76, "region", creationDate));
            //"Employee",
            perms.AddRange(BuildCrudDefault(28, 82, "employee", creationDate));
            //"Customer",
            perms.AddRange(BuildCrudDefault(29, 85, "customer", creationDate));
            //"General Setting",
            perms.AddRange(BuildCrudDefault(30, 88, "general_setting", creationDate));
            //"POS Config",
            perms.AddRange(BuildCrudDefault(31, 91, "pos_config", creationDate));
            //"Report Settings",
            //perms.AddRange(BuildCrudDefault(32, 94, "report_settings", creationDate));
            //"Home Screen",
            perms.AddRange(BuildCrudDefault(33, 97, "home_screen", creationDate));
            //Scale Home FAV
            perms.AddRange(BuildCrudDefault(37, 109, "scale_home_fav", creationDate));
            //"Fee",
            perms.AddRange(BuildCrudDefault(40, 118, "fee", creationDate));
            //Bin Location
            perms.AddRange(BuildCrudDefault(43, 121, "bin_location", creationDate));
            //Inventory
            perms.AddRange(BuildCrudDefault(44, 124, "inventory", creationDate));
            //Loyalty discount
            perms.AddRange(BuildCrudDefault(45, 144, "loyalty_discount", creationDate));
            //Vendor Order
            perms.AddRange(BuildCrudDefault(46, 147, "vendor_order", creationDate));

            perms.Add(new Permission { FunctionGroupId = 22, Id = 130, Name = "Assign Store", Code = "associate_product_store", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            perms.Add(new Permission { FunctionGroupId = 22, Id = 131, Name = "Assign Vendor", Code = "associate_product_vendor", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            // perms.Add(new Permission { FunctionGroupId = 22, Id = 132, Name = "Assign Image", Code = "associate_product_image", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            perms.Add(new Permission { FunctionGroupId = 22, Id = 133, Name = "Assign Scale Label Definitiion", Code = "associate_product_scale_label_definition", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            // perms.Add(new Permission { FunctionGroupId = 3, Id = 134, Name = "Assign Image", Code = "associate_category_image", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            // perms.Add(new Permission { FunctionGroupId = 23, Id = 135, Name = "Assign Image", Code = "associate_department_image", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            // perms.Add(new Permission { FunctionGroupId = 8, Id = 136, Name = "Assign Image", Code = "associate_scale_category_image", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            perms.Add(new Permission { FunctionGroupId = 41, Id = 137, Name = "Add / Edit", Code = "add_edit_device", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            perms.Add(new Permission { FunctionGroupId = 41, Id = 138, Name = "View", Code = "view_device", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            perms.Add(new Permission { FunctionGroupId = 27, Id = 139, Name = "View", Code = "view_report", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            
            perms.Add(new Permission { FunctionGroupId = 42, Id = 140, Name = "Close Day", Code = "close_eod", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });
            perms.Add(new Permission { FunctionGroupId = 42, Id = 141, Name = "Manage", Code = "manage_eod", CreatedAt = creationDate, UpdatedAt = creationDate, UserCreatorId = SqlServerContext.SeedUserId, State = true });

            
            //Reason codes
            perms.AddRange(BuildCrudDefault(47, 150, "reason_codes", creationDate));
            
            //"Rancher",
            perms.AddRange(BuildCrudDefault(48, 153, "rancher", creationDate));
            //"Breed",
            perms.AddRange(BuildCrudDefault(49, 156, "breed", creationDate));
            //"Grind",
            perms.AddRange(BuildCrudDefault(50, 159, "grind", creationDate));
            //Animal 
            perms.AddRange(BuildCrudDefault(51, 162, "animal", creationDate));
            //Schedule 
            perms.AddRange(BuildCrudDefault(52, 165, "schedule", creationDate));
            //Scalendar
            perms.AddRange(BuildCrudDefault(53, 168, "scalendar", creationDate));
            
            #endregion Backoffice

            #region POS

            //Tax
            perms.Add(Build(101, 1001, "Sell", "allow_sell", creationDate));

            perms.Add(Build(102, 1002, "Void", "posbutton_void", creationDate));
            perms.Add(Build(102, 1003, "Cancel", "posbutton_cancel", creationDate));
            perms.Add(Build(102, 1004, "Snap/EBT", "posbutton_snap", creationDate));
            perms.Add(Build(102, 1005, "Discount", "posbutton_discount", creationDate));
            perms.Add(Build(102, 1006, "Gift Card", "posbutton_gift", creationDate));
            perms.Add(Build(102, 1007, "Suspend/Resume", "posbutton_suspend", creationDate));
            perms.Add(Build(102, 1008, "Bottle Refund", "posbutton_bottle", creationDate));
            perms.Add(Build(102, 1009, "Return", "posbutton_return", creationDate));
            perms.Add(Build(102, 1010, "Zero Scale", "posbutton_zero", creationDate));
            perms.Add(Build(102, 1011, "No Sale", "posbutton_nosale", creationDate));
            perms.Add(Build(102, 1012, "Reprint Receipt", "posbutton_reprint", creationDate));
            perms.Add(Build(102, 1013, "Paid Out", "posbutton_paidout", creationDate));
            
            perms.Add(Build(102, 1014, "EBT Check Balance", "posbutton_ebtbalance", creationDate));
            perms.Add(Build(102, 1015, "Remove Service Fee", "posbutton_removeservicefee", creationDate));
            perms.Add(Build(102, 1016, "Tax override", "posbutton_taxoverride", creationDate));
            
            perms.Add(Build(101, 1017, "Remove product from sale", "allow_remove_product", creationDate));
            
            #endregion POS

            builder.HasData(perms);

            #endregion Seed
        }

        private List<Permission> BuildCrudDefault(long function, long initialId, string code, DateTime creationDate)
        {
            return new()
            {
                Build(function, initialId, "View", $"view_{code}", creationDate),
                Build(function, initialId + 1, "Add/Edit", $"add_edit_{code}", creationDate),
                Build(function, initialId + 2, "Delete", $"delete_{code}", creationDate)
            };
        }

        private Permission Build(
            long function,
            long id,
            string name,
            string code,
            DateTime creationDate)
        {
            return new()
            {
                Id = id,
                Name = name,
                Code = code,
                State = true,
                FunctionGroupId = function,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = creationDate,
                UpdatedAt = creationDate
            };
        }
    }
}