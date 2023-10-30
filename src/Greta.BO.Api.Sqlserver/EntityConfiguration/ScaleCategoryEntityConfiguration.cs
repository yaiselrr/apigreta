using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ScaleCategoryEntityConfiguration : IEntityTypeConfiguration<ScaleCategory>
    {
        public void Configure(EntityTypeBuilder<ScaleCategory> builder)
        {
            builder.ConfigurationBase<long, string, ScaleCategory>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasIndex(p => p.CategoryId).IsUnique();
            builder.HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            #region Seed

            // var categories = new List<ScaleCategory>();
            // for (var i = 0; i < 1; i++)
            // {
            //     var category = new ScaleCategory()
            //     {
            //         Id = i + 1,
            //         State = true,
            //         Name = "Scale Category " + i,
            //         DepartmentId = 1,
            //         UserCreatorId = SqlServerContext.SeedUserId,
            //     };
            //     categories.Add(category);
            // }

            // builder.HasData(categories);

            #endregion Seed
        }
    }
}