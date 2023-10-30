using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class CSVMappingEntityConfiguration : IEntityTypeConfiguration<CSVMapping>
    {
        public void Configure(EntityTypeBuilder<CSVMapping> builder)
        {
            builder.ConfigurationBase<long, string, CSVMapping>();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.MapperJson).IsRequired();
            builder.Property(x => x.ModelImport).IsRequired();
        }
    }
}