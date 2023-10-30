using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class FamilyEntityConfiguration : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.ConfigurationBase<long, string, Family>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();

            builder.HasMany(x => x.Discounts);
            
        }
    }
}