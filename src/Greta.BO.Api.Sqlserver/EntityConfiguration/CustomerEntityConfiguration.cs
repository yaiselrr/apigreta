using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver.Extensions;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ConfigurationBase<long, string, Customer>();
            builder.ConfigurationLocalizationBase<long, string, Customer>();

            builder.Property(x => x.FirstName).HasColumnType("varchar(64)").IsRequired();
            builder.Property(x => x.LastName).HasColumnType("varchar(64)").IsRequired();
            builder.Property(x => x.Phone).HasColumnType("varchar(250)").IsRequired();
            builder.Property(x => x.Email).HasColumnType("varchar(100)");
            builder.Property(x => x.TaxID).HasColumnType("varchar(64)");

            builder.Property(x => x.StoreCredit).HasPrecision(18, 2);
            
            builder.HasMany(x => x.MixAndMatches)
                .WithMany(x => x.Customers);
            
            builder.HasMany(x => x.Discounts)
                .WithMany(x => x.Customers);
            
        }
    }
}