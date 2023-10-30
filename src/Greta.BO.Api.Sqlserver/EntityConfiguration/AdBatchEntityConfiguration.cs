using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver.Extensions;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    /// <inheritdoc />
    public class AdBatchEntityConfiguration : IEntityTypeConfiguration<AdBatch>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<AdBatch> builder)
        {
            builder.ConfigurationBase<long, string, AdBatch>();
            builder.Property(x => x.StartTime);//.UsesUtc();
            builder.Property(x => x.EndTime);//.UsesUtc();
        }
    }
}