using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    internal class BreedEntityConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ConfigurationBase<long, string, Breed>();

            builder.Property(x => x.Name).HasColumnType("varchar(60)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(x => x.Maxx).HasColumnType("numeric(18,2)");

            var Breed = new Breed
            {
                Id = 1,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Name = "Angus",
                Maxx = 0
            };
            builder.HasData(new List<Breed> {Breed});
        }
    }
}