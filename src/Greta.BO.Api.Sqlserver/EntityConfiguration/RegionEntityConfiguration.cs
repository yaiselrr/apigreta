using System;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class RegionEntityConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ConfigurationBase<long, string, Region>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            //builder.HasQueryFilter(e => e.Id != 1);

            builder.HasData(new Region
            {
                Id = 1,
                Name = "Default",
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            });

        }
    }
}