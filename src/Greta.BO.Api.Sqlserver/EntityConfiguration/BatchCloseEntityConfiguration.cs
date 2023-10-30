using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class BatchCloseEntityConfiguration: IEntityTypeConfiguration<BatchClose>
    {
        public void Configure(EntityTypeBuilder<BatchClose> builder)
        {
            builder.ConfigurationBase<long, string, BatchClose>(userRequired: false);
            
            builder.Property(x => x.EBTAmount).HasColumnType("numeric(18,2)");
            builder.Property(x => x.HostTotalsAmount1).HasColumnType("numeric(18,2)");
            builder.Property(x => x.HostTotalsAmount5).HasColumnType("numeric(18,2)");
            
            builder.HasOne(x => x.Device).WithMany(x => x.BatchClose).HasForeignKey(x => x.DeviceId).OnDelete(DeleteBehavior.Cascade);

            // builder.HasDiscriminator(b => b.Type)
            //     .HasValue<Batch>(RetailPriceBatchType.Base)
            //     .HasValue<PriceBatch>(RetailPriceBatchType.Batch)
            //     .HasValue<AdBatch>(RetailPriceBatchType.AD)
            //     .IsComplete(false);
            //
            // builder.HasMany(x => x.Stores).WithMany(x => x.Batchs);
        }
    }
}