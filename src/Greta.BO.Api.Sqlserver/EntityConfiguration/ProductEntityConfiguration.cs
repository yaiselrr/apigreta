using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ConfigurationBase<long, string, Product>();


            //builder.HasDiscriminator<ProductType>("ProductType")
            // builder.HasDiscriminator(b => b.ProductType)
            //     .HasValue<Product>(ProductType.SPT)
            //     .HasValue<ScaleProduct>(ProductType.SLP)
            //     .HasValue<KitProduct>(ProductType.KPT)
            //     .IsComplete(true);

            builder.Property(x => x.UPC).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(p => p.UPC).IsUnique();
            builder.Property(x => x.Name).HasColumnType("varchar(200)").IsRequired();
            builder.HasIndex(p => p.Name);//.IsUnique();

            builder.Property(x => x.Tare1).HasColumnType("numeric(18,2)");
           

            builder.HasOne(x => x.Category).WithMany(x=>x.Products).HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Department).WithMany(x=>x.Products).HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Family).WithMany(x=> x.Products).HasForeignKey(x => x.FamilyId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.DefaulShelfTag).WithMany().HasForeignKey(x => x.DefaulShelfTagId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}