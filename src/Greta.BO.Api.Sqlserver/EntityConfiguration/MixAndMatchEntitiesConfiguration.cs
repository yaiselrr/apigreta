using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class MixAndMatchEntitiesConfiguration : IEntityTypeConfiguration<MixAndMatch>
    {
        public void Configure(EntityTypeBuilder<MixAndMatch> builder)
        {
            builder.ConfigurationBase<long, string, MixAndMatch>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(x => x.Amount).HasColumnType("numeric(18,2)");


            builder.HasOne(x => x.ProductBuy)
                .WithMany(x => x.BuyMixAndMatchs)
                .HasForeignKey(x => x.ProductBuyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Families)
                .WithMany(x => x.MixAndMatchs);

            builder.HasMany(x => x.Products)
                .WithMany(x => x.MixAndMatchs);
        }
    }
}