using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ClientApplicationEntityConfiguration : IEntityTypeConfiguration<ClientApplication>
    {
        public void Configure(EntityTypeBuilder<ClientApplication> builder)
        {
            builder.ConfigurationBase<long, string, ClientApplication>();

            builder.Property(x => x.Name).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();


            builder
                .HasMany(x => x.FunctionGroups)
                .WithOne(x => x.ClientApplication)
                .HasForeignKey(x => x.ClientApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            #region Seed

            var creationDate = new DateTime(2021, 3, 10);
            builder.HasData(new List<ClientApplication>
            {
                new()
                {
                    Id = 1,
                    Name = "Back Office",
                    State = true,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = creationDate,
                    UpdatedAt = creationDate
                },
                new()
                {
                    Id = 2,
                    Name = "POS",
                    State = true,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = creationDate,
                    UpdatedAt = creationDate
                }
            });

            #endregion Seed
        }
    }
}