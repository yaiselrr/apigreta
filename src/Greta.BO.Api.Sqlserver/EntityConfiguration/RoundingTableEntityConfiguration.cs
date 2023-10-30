using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration;

public class RoundingTableEntityConfiguration: IEntityTypeConfiguration<RoundingTable>
{
    public void Configure(EntityTypeBuilder<RoundingTable> builder)
    {
        builder.ConfigurationBase<long, string, RoundingTable>();

        builder.Property(x => x.ChangeBy).IsRequired();
        builder.Property(x => x.EndWith).IsRequired();
        builder.HasData(new List<RoundingTable>(){
            new RoundingTable
            {
                Id = 1,
                ChangeBy = 9,
                EndWith = 0,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 2,
                ChangeBy = 9,
                EndWith = 1,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 3,
                ChangeBy = 9,
                EndWith = 2,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 4,
                ChangeBy = 9,
                EndWith = 3,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 5,
                ChangeBy = 9,
                EndWith = 4,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 6,
                ChangeBy = 9,
                EndWith = 5,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 7,
                ChangeBy = 9,
                EndWith = 6,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 8,
                ChangeBy = 9,
                EndWith = 7,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 9,
                ChangeBy = 9,
                EndWith = 8,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            },
            new RoundingTable
            {
                Id = 10,
                ChangeBy = 9,
                EndWith = 9,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                CreatedAt = new DateTime(2021, 3, 10),
                UpdatedAt = new DateTime(2021, 3, 10)
            }
        });
    }
}