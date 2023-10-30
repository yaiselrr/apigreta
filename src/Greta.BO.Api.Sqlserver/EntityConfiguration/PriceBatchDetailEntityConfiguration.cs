using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class PriceBatchDetailEntityConfiguration : IEntityTypeConfiguration<PriceBatchDetail>
    {
        public void Configure(EntityTypeBuilder<PriceBatchDetail> builder)
        {
            builder.ConfigurationBase<long, string, PriceBatchDetail>();

            builder.Property(x => x.Price).HasColumnType("numeric(18,2)").IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.PriceBatchDetails).HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Family)
                .WithMany(x => x.PriceBatchDetails).HasForeignKey(x => x.FamilyId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(x => x.Category)
                .WithMany(x => x.PriceBatchDetails).HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Header)
                .WithMany(x => x.PriceBatchDetails).HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}