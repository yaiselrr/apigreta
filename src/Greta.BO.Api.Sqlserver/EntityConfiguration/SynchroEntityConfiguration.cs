using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class SynchroEntityConfiguration : IEntityTypeConfiguration<Synchro>
    {
        public void Configure(EntityTypeBuilder<Synchro> builder)
        {
            builder.ConfigurationBase<long, string, Synchro>(false);

            builder.HasMany(x => x.SynchroDetails)
                .WithOne()
                .HasForeignKey(x => x.SynchroId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}