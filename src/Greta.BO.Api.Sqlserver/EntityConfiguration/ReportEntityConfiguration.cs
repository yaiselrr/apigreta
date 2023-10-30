using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ReportEntityConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Report");
            builder.ConfigurationBase<long, string, Report>();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.GuidId).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasIndex(p => p.GuidId).IsUnique();
            
           
        }
    }
}
