using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class VendorContactEntityConfiguration : IEntityTypeConfiguration<VendorContact>
    {
        public void Configure(EntityTypeBuilder<VendorContact> builder)
        {
            builder.ConfigurationBase<long, string, VendorContact>(false);

            builder.Property(x => x.Contact).HasColumnType("varchar(64)").IsRequired();
            builder.Property(x => x.Phone).HasColumnType("varchar(14)").IsRequired();
            builder.Property(x => x.Email).HasColumnType("varchar(60)").IsRequired();
            builder.Property(x => x.Fax).HasColumnType("varchar(60)");

            builder.HasOne(x => x.Vendor).WithMany(x => x.VendorContacts).HasForeignKey(x => x.VendorId);
        }
    }
}