using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ScaleLabelDefinitionEntityConfiguration : IEntityTypeConfiguration<ScaleLabelDefinition>
    {
        public void Configure(EntityTypeBuilder<ScaleLabelDefinition> builder)
        {
            builder.ConfigurationBase<long, string, ScaleLabelDefinition>();

            // builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            // builder.HasIndex(p => p.Name).IsUnique();
            // // builder.HasMany("ScaleProduct");
            // builder.HasMany(x => x.ScaleLabelDefinitions);

            // builder.HasOne(x => x.ScaleBrand).WithMany().HasForeignKey(x => x.ScaleBrandId)
            //     .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.ScaleProduct).WithMany(x => x.ScaleLabelDefinitions)
                .HasForeignKey(x => x.ScaleProductId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.ScaleLabelType1).WithMany().HasForeignKey(x => x.ScaleLabelType1Id)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.ScaleLabelType2).WithMany().HasForeignKey(x => x.ScaleLabelType2Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(x => x.ImportLabel);
        }
    }
}