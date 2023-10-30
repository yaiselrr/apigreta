using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class QtyHandTrackEntityConfiguration : IEntityTypeConfiguration<QtyHandTrack>
    {
        public void Configure(EntityTypeBuilder<QtyHandTrack> builder)
        {
            builder.ConfigurationBase<long, string, QtyHandTrack>();

            builder.Property(x => x.OldQtyHand).HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(x => x.NewQtyHand).HasColumnType("numeric(18,2)").IsRequired();
        }
    }
}