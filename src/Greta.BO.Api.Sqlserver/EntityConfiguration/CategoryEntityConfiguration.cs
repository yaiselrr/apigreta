using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ConfigurationBase<long, string, Category>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name);//.IsUnique();
            builder.Property(x => x.Description).HasColumnType("varchar(254)");
            builder.HasIndex(p => p.CategoryId).IsUnique();


            builder.HasOne(x => x.DefaulShelfTag)
                .WithMany()
                .HasForeignKey(x => x.DefaulShelfTagId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Department)
                .WithMany(x=>x.Categories)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Taxs)
                .WithMany(x => x.Categories);

        }
    }
}