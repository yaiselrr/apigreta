using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ScalendarEntityConfiguration : IEntityTypeConfiguration<Scalendar>
    {
        public void Configure(EntityTypeBuilder<Scalendar> builder)
        {
            builder.ConfigurationBase<long, string, Scalendar>();

            builder.HasIndex(p => new {p.Day}).IsUnique();
            builder.HasIndex(p => p.DayId).IsUnique();
            builder.HasMany(x => x.Breeds).WithMany(x => x.Scalendars);

            var monday = new Scalendar
            {
                Id = 1,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Day = "Monday",
                DayId = 1,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };
            var tuesday = new Scalendar
            {
                Id = 2,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Day = "Tuesday",
                DayId = 2,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };
            var wednesday = new Scalendar
            {
                Id = 3,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Day = "Wednesday",
                DayId = 3,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };
            var thursday = new Scalendar
            {
                Id = 4,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Day = "Thursday",
                DayId = 4,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };
            var friday = new Scalendar
            {
                Id = 5,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Day = "Friday",
                DayId = 5,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };
            var saturday = new Scalendar
            {
                Id = 6,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Day = "Saturday",
                DayId = 6,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };
            var sunday = new Scalendar
            {
                Id = 7,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Day = "Sunday",
                DayId = 0,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt,
            };

            builder.HasData(new List<Scalendar> {monday, tuesday, wednesday, thursday, friday, saturday, sunday});
           
        }
    }
}