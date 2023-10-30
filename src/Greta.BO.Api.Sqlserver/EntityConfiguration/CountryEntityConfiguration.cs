using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    internal class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ConfigurationBase<long, string, Country>();

            builder.Property(x => x.Name).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            var country = new Country
            {
                Id = 1,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Name = "United States",
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };
            builder.HasData(new List<Country> {country});
        }
    }
}