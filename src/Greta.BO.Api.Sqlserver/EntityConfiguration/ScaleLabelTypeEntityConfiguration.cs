using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ScaleLabelTypeEntityConfiguration : IEntityTypeConfiguration<ScaleLabelType>
    {
        public void Configure(EntityTypeBuilder<ScaleLabelType> builder)
        {
            builder.ConfigurationBase<long, string, ScaleLabelType>();

            builder.Property(x => x.Name).HasColumnType("varchar(64)").IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            //builder.HasOne(x => x.ScaleLabelDesign).WithOne(x => x.ScaleLabelType).HasForeignKey<ScaleLabelDesign>(x=> x.ScaleLabelTypeId);
        }
    }
    
    // public class ScaleLabelDesignEntityConfiguration : IEntityTypeConfiguration<ScaleLabelDesign>
    // {
    //     public void Configure(EntityTypeBuilder<ScaleLabelDesign> builder)
    //     {
    //         builder.ConfigurationBase<long, string, ScaleLabelDesign>();
    //
    //         
    //     }
    // }
}