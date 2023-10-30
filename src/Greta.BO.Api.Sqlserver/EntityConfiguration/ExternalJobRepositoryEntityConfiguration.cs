using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ExternalJobRepositoryEntityConfiguration: IEntityTypeConfiguration<ExternalJob>
    {
        public void Configure(EntityTypeBuilder<ExternalJob> builder)
        {
            builder.ConfigurationBase<long, string, ExternalJob>();
        }
    }
}