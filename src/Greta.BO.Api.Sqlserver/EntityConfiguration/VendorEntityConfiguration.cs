using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver.Extensions;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class VendorEntityConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ConfigurationBase<long, string, Vendor>();

            builder.ConfigurationLocalizationBase<long, string, Vendor>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.Property(x => x.Note).HasColumnType("varchar(254)");
            builder.Property(x => x.MinimalOrder).HasColumnType("numeric(15,3)");

            builder.HasMany(x => x.VendorProducts);
            builder.HasMany(x => x.VendorContacts);
        }
    }
}