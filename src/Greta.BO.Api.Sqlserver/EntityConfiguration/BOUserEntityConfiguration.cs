using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class BOUserEntityConfiguration : IEntityTypeConfiguration<BOUser>
    {
        public void Configure(EntityTypeBuilder<BOUser> builder)
        {
            builder.ConfigurationBase<long, string, BOUser>();

            builder.HasIndex(p => p.UserId).IsUnique();

            builder.HasOne(x => x.BOProfile).WithMany().HasForeignKey(x => x.BOProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.POSProfile).WithMany().HasForeignKey(x => x.POSProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Stores).WithMany(x => x.Employees);

            #region Seed

            // var admin = new BOUser()
            // {
            //     Id = 1,
            //     State = true,
            //     UserId = SqlServerContext.SeedUserId,
            //     BOProfileId = 1,
            //     POSProfileId = 2,
            //     RoleId = 1,
            //     UserCreatorId = SqlServerContext.SeedUserId
            // };

            // builder.HasData(new List<BOUser>() { admin });

            #endregion Seed
        }
    }
}