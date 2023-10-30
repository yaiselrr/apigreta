using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class CutListTemplateEntitiesConfiguration : IEntityTypeConfiguration<CutListTemplate>
    {
        public void Configure(EntityTypeBuilder<CutListTemplate> builder)
        {
            builder.ConfigurationBase<long, string, CutListTemplate>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();         

            builder.HasMany(x => x.ScaleProducts)
                .WithMany(x => x.CutListTemplates);           
        }
    }
}