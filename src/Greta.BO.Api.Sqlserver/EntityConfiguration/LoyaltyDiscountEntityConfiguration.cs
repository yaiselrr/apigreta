using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class LoyaltyDiscountEntityConfiguration: IEntityTypeConfiguration<LoyaltyDiscount>
    {
        public void Configure(EntityTypeBuilder<LoyaltyDiscount> builder)
        {
            builder.ConfigurationBase<long, string, LoyaltyDiscount>();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Value).HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(x => x.Maximum).HasColumnType("numeric(18,2)").IsRequired();

            builder
                .HasOne(x => x.Store)
                .WithOne(x => x.LoyaltyDiscount)
                .HasForeignKey<LoyaltyDiscount>(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}