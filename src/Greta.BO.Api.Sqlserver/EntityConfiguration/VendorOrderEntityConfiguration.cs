using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver.Extensions;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class VendorOrderEntityConfiguration: IEntityTypeConfiguration<VendorOrder>
    {
        public void Configure(EntityTypeBuilder<VendorOrder> builder)
        {
            builder.ConfigurationBase<long, string, VendorOrder>();

            builder.HasOne(x => x.Vendor)
                .WithMany(x => x.VendorOrders)
                .HasForeignKey(x => x.VendorId);
            builder.HasOne(x => x.Store)
                .WithMany(x => x.VendorOrders)
                .HasForeignKey(x => x.StoreId);
            builder.HasOne(x => x.User)
                .WithMany(x => x.VendorOrders)
                .HasForeignKey(x => x.UserId);

        }
    }
}