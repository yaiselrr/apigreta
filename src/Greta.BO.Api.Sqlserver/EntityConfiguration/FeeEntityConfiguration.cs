using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class FeeEntityConfiguration : IEntityTypeConfiguration<Fee>
    {
        public void Configure(EntityTypeBuilder<Fee> builder)
        {
            builder.ConfigurationBase<long, string, Fee>();

            builder.Property(x => x.Name).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(x => x.Amount).HasColumnType("numeric(18,2)");

            builder.HasMany(x => x.Products)
                .WithMany(x => x.Fees);

            builder.HasMany(x => x.Families)
                .WithMany(x => x.Fees);

            builder.HasMany(x => x.Categories)
                .WithMany(x => x.Fees);
        }
    }
}