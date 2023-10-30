using System;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ConfigurationBase<long, string, Role>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasData(new Role
            {
                Id = 1,
                Name = "Administrator",
                AllStores = true,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            });
        }
    }
}