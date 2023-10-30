using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ChangePriceReasonCodeEntityConfiguration: IEntityTypeConfiguration<ChangePriceReasonCode>
    {
        public void Configure(EntityTypeBuilder<ChangePriceReasonCode> builder)
        {
            builder.ConfigurationBase<long, string, ChangePriceReasonCode>(userRequired:false);

            builder.Property(x => x.NewPrice).HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(x => x.OldPrice).HasColumnType("numeric(18,2)").IsRequired();
        }
    }
}