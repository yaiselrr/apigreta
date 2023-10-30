using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration;

public class CutListDetailEntityConfiguration : IEntityTypeConfiguration<CutListDetail>
{
    public void Configure(EntityTypeBuilder<CutListDetail> builder)
    {
        builder.ConfigurationBase<long, string, CutListDetail>();
        
        builder.HasOne(x => x.CutList).WithMany(x => x.CutListDetails).HasForeignKey(x => x.CutListId).OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
        builder.Property(x => x.Pack).HasColumnType("integer");
        builder.Property(x => x.Thick).HasColumnType("numeric(18,2)");
    }
}