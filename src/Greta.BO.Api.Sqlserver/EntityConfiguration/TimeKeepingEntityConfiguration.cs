using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration;

public class TimeKeepingEntityConfiguration : IEntityTypeConfiguration<TimeKeeping>
{
    public void Configure(EntityTypeBuilder<TimeKeeping> builder)
    {
        builder.ConfigurationBase<long, string, TimeKeeping>();
    }
}