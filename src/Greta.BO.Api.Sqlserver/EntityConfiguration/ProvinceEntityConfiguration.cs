using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    internal class ProvinceEntityConfiguration : IEntityTypeConfiguration<Province>
    {
        
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ConfigurationBase<long, string, Province>();

            builder.Property(x => x.Name).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);

            var provinces = new List<Province>();
            var count = 3;
            provinces.Add(CreateUSProvince(1, "Florida"));
            provinces.Add(CreateUSProvince(2, "Texas"));
            provinces.Add(CreateUSProvince(count++, "Alabama"));
            provinces.Add(CreateUSProvince(count++, "Alaska"));
            provinces.Add(CreateUSProvince(count++, "Arizona"));
            provinces.Add(CreateUSProvince(count++, "Arkansas"));
            provinces.Add(CreateUSProvince(count++, "California"));
            provinces.Add(CreateUSProvince(count++, "Colorado"));
            provinces.Add(CreateUSProvince(count++, "Connecticut"));
            provinces.Add(CreateUSProvince(count++, "Delaware"));
            provinces.Add(CreateUSProvince(count++, "Georgia"));
            provinces.Add(CreateUSProvince(count++, "Hawaii"));
            provinces.Add(CreateUSProvince(count++, "Idaho"));
            provinces.Add(CreateUSProvince(count++, "Illinois"));
            provinces.Add(CreateUSProvince(count++, "Indiana"));
            provinces.Add(CreateUSProvince(count++, "Iowa"));
            provinces.Add(CreateUSProvince(count++, "Kansas"));
            provinces.Add(CreateUSProvince(count++, "Kentucky"));
            provinces.Add(CreateUSProvince(count++, "Louisiana"));
            provinces.Add(CreateUSProvince(count++, "Maine"));
            provinces.Add(CreateUSProvince(count++, "Maryland"));
            provinces.Add(CreateUSProvince(count++, "Massachusetts"));
            provinces.Add(CreateUSProvince(count++, "Michigan"));
            provinces.Add(CreateUSProvince(count++, "Minnesota"));
            provinces.Add(CreateUSProvince(count++, "Mississippi"));
            provinces.Add(CreateUSProvince(count++, "Missouri"));
            provinces.Add(CreateUSProvince(count++, "Montana"));
            provinces.Add(CreateUSProvince(count++, "Nebraska"));
            provinces.Add(CreateUSProvince(count++, "Nevada"));
            provinces.Add(CreateUSProvince(count++, "New Hampshire"));
            provinces.Add(CreateUSProvince(count++, "New Jersey"));
            provinces.Add(CreateUSProvince(count++, "New Mexico"));
            provinces.Add(CreateUSProvince(count++, "New York"));
            provinces.Add(CreateUSProvince(count++, "North Carolina"));
            provinces.Add(CreateUSProvince(count++, "North Dakota"));
            provinces.Add(CreateUSProvince(count++, "Ohio"));
            provinces.Add(CreateUSProvince(count++, "Oklahoma"));
            provinces.Add(CreateUSProvince(count++, "Oregon"));
            provinces.Add(CreateUSProvince(count++, "Pennsylvania"));
            provinces.Add(CreateUSProvince(count++, "Rhode Island"));
            provinces.Add(CreateUSProvince(count++, "South Carolina"));
            provinces.Add(CreateUSProvince(count++, "South Dakota"));
            provinces.Add(CreateUSProvince(count++, "Tennessee"));
            provinces.Add(CreateUSProvince(count++, "Utah"));
            provinces.Add(CreateUSProvince(count++, "Vermont"));
            provinces.Add(CreateUSProvince(count++, "Virginia"));
            provinces.Add(CreateUSProvince(count++, "Washington"));
            provinces.Add(CreateUSProvince(count++, "West Virginia"));
            provinces.Add(CreateUSProvince(count++, "Wisconsin"));
            provinces.Add(CreateUSProvince(count++, "Wyoming"));
            
            
            builder.HasData(provinces);
        }

        private Province CreateUSProvince(long id, string name) => new Province
            {
                Id = id,
                CountryId = 1,
                State = true,
                UserCreatorId = SqlServerContext.SeedUserId,
                Name = name,
                CreatedAt = SqlServerContext.createdAt,
                UpdatedAt = SqlServerContext.createdAt
            };
    }
}