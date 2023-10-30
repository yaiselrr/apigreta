using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    internal class TaxEntityConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.ConfigurationBase<long, string, Tax>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(x => x.Description).HasColumnType("varchar(254)").IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Value).IsRequired();

            builder.HasMany(x => x.Stores);

            builder.HasMany(x => x.StoreProducts);

        }
    }
}