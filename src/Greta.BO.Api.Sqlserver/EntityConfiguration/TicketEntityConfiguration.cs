using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    class TicketEntityConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ConfigurationBase<long, string, Ticket>();

            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.GuidId).IsRequired();
            builder.HasIndex(x => x.GuidId).IsUnique();
        }
    }
}