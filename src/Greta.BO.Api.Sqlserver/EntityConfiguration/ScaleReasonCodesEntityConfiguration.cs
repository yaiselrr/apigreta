using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ScaleReasonCodesEntityConfiguration : IEntityTypeConfiguration<ScaleReasonCodes>
    {
        public void Configure(EntityTypeBuilder<ScaleReasonCodes> builder)
        {
            builder.ConfigurationBase<long, string, ScaleReasonCodes>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            //builder.HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentId)
                //.OnDelete(DeleteBehavior.NoAction);
        }
    }
}
