using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver.Extensions;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class PriceBatchEntityConfiguration : IEntityTypeConfiguration<PriceBatch>
    {
        public void Configure(EntityTypeBuilder<PriceBatch> builder)
        {
            builder.ConfigurationBase<long, string, PriceBatch>();
            builder.Property(x => x.StartTime).UsesUtc();

            // builder.HasDiscriminator(b => b.Type)
            //     .HasValue<PriceBatch>(RetailPriceBatchType.Batch)
            //     .HasValue<AdBatch>(RetailPriceBatchType.AD)
            //     .IsComplete(true);

            // builder.HasOne(x => x.Store)
            //    .WithMany().HasForeignKey(x => x.StoreId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}