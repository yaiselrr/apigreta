using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class BatchEntityConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.ConfigurationBase<long, string, Batch>();

            builder.HasDiscriminator(b => b.Type)
                .HasValue<Batch>(RetailPriceBatchType.Base)
                .HasValue<PriceBatch>(RetailPriceBatchType.Batch)
                .HasValue<AdBatch>(RetailPriceBatchType.AD)
                .IsComplete(false);

            builder.HasMany(x => x.Stores).WithMany(x => x.Batchs);
        }
    }
}