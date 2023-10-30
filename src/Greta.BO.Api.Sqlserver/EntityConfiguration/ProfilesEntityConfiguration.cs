using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ProfilesEntityConfiguration : IEntityTypeConfiguration<Profiles>
    {
        public void Configure(EntityTypeBuilder<Profiles> builder)
        {
            builder.ConfigurationBase<long, string, Profiles>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();


            // builder
            //     .HasOne(x => x.Profile)
            //     .WithMany(x => x.FunctionGroups)
            //     .HasForeignKey(x => x.ProfileId)
            //     .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(x => x.Application)
                .WithMany()
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(x => x.Permissions)
                .WithMany(x => x.Profiles);

            #region Seed

            Seed(builder);

            #endregion Seed
        }

        private void Seed(EntityTypeBuilder<Profiles> builder)
        {
            var creationDate = new DateTime(2021, 3, 10);
            var result = new List<Profiles>
            {
                new()
                {
                    Id = 1L,
                    Name = "Administrator",
                    ApplicationId = 1L,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = creationDate,
                    UpdatedAt = creationDate,
                    State = true
                },
                new()
                {
                    Id = 2L,
                    Name = "Manager",
                    ApplicationId = 2L,
                    UserCreatorId = SqlServerContext.SeedUserId,
                    CreatedAt = creationDate,
                    UpdatedAt = creationDate,
                    State = true
                }
            };

            builder.HasData(result);
        }
    }
}