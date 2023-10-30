using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver.Extensions;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    // public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    // {
    //     public void Configure(EntityTypeBuilder<Employee> builder)
    //     {
    //         builder.ConfigurationBase<long, string, Employee>();
    //         builder.ConfigurationLocalizationBase<long, string, Employee>();
    //
    //         builder.Property(x => x.FirstName).HasColumnType("varchar(64)").IsRequired();
    //         builder.Property(x => x.LastName).HasColumnType("varchar(64)").IsRequired();
    //         builder.Property(x => x.Phone).HasColumnType("varchar(20)").IsRequired();
    //         builder.Property(x => x.Email).HasColumnType("varchar(100)").IsRequired();
    //         builder.Property(x => x.Password).HasColumnType("varchar(6)").IsRequired();
    //
    //         builder.HasMany(x => x.Stores).WithMany(x => x.Employees);
    //
    //     }
    // }
}