using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    internal class RancherEntityConfiguration : IEntityTypeConfiguration<Rancher>
    {
        public void Configure(EntityTypeBuilder<Rancher> builder)
        {
            builder.ConfigurationBase<long, string, Rancher>();

            builder.Property(x => x.Name).HasColumnType("varchar(60)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            var rancher = new Rancher
            {
                Id = 1,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Name = "Triple Greta"
            };
            builder.HasData(new List<Rancher> {rancher});
        }
    }
}