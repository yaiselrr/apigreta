using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ScaleHomeFavEntityConfiguration : IEntityTypeConfiguration<ScaleHomeFav>
    {
        public void Configure(EntityTypeBuilder<ScaleHomeFav> builder)
        {
            builder.ConfigurationBase<long, string, ScaleHomeFav>();

            builder.HasIndex(p => new {p.StoreId, p.DepartmentId}).IsUnique();
            builder.HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.ScaleProducts).WithMany(x => x.ScaleHomeFavs);
        }
    }
}