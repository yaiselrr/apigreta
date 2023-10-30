using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class SynchroDetailEntityConfiguration : IEntityTypeConfiguration<SynchroDetail>
    {
        public void Configure(EntityTypeBuilder<SynchroDetail> builder)
        {
            builder.ConfigurationBase<long, string, SynchroDetail>(false);
        }
    }
}