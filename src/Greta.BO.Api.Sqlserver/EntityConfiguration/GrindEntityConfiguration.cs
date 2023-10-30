using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    internal class GrindEntityConfiguration : IEntityTypeConfiguration<Grind>
    {
        public void Configure(EntityTypeBuilder<Grind> builder)
        {
            builder.ConfigurationBase<long, string, Grind>();

            builder.Property(x => x.Name).HasColumnType("varchar(60)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}