using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class DepartmentEntityConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ConfigurationBase<long, string, Department>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasIndex(p => p.DepartmentId).IsUnique();
            // builder.HasMany(x => x.Products);

            #region Seed

            var departments = new List<Department>();
            for (var i = 0; i < 1; i++)
            {
                var department = new Department
                {
                    Id = i + 1,
                    State = true,
                    DepartmentId = i + 1,
                    Perishable = false,
                    Name = "Default",
                    UserCreatorId = SqlServerContext.SeedUserId
                };
                departments.Add(department);
            }

            builder.HasData(departments);

            #endregion Seed
        }
    }
}