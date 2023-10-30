using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration;

public class CutListEntityConfiguration : IEntityTypeConfiguration<CutList>
{
    public void Configure(EntityTypeBuilder<CutList> builder)
    {
        builder.ConfigurationBase<long, string, CutList>();

        builder.HasIndex(x => new { x.AnimalId, x.CustomerId }).IsUnique();
        
        builder.Property(x => x.CutListType).HasColumnType("integer");
        builder.Property(x => x.SpecialInstruction).HasColumnType("text");
        
        builder.HasOne(x => x.Animal).WithMany().HasForeignKey( x=>x.AnimalId).OnDelete(DeleteBehavior.NoAction).IsRequired();
        builder.HasOne(x => x.Customer).WithMany().HasForeignKey( x=>x.CustomerId).OnDelete(DeleteBehavior.NoAction).IsRequired();

    }
}