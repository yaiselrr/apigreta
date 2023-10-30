using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class BinLocationEntityConfiguration : IEntityTypeConfiguration<BinLocation>
    {
        public void Configure(EntityTypeBuilder<BinLocation> builder)
        {
            builder.ConfigurationBase<long, string, BinLocation>();
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Name).HasColumnType("varchar(64)");
            builder.Property(x => x.Aisle).HasColumnType("integer").IsRequired();
            builder.Property(x => x.Side).HasColumnType("integer").IsRequired();
            builder.Property(x => x.Section).HasColumnType("integer").IsRequired();
            builder.Property(x => x.Shelf).HasColumnType("integer").IsRequired();
            builder.Property(x => x.Store).HasColumnType("long").IsRequired();
            
            
            //builder.HasOne(x => x.BinLocation).WithMany().HasForeignKey(sp => sp.BinLocationId).OnDelete(DeleteBehavior.NoAction);
            
            //builder.HasOne(x => x.Store);
            /*builder.HasOne(x => x.Store)
                .WithMany()
                .HasForeignKey(x => x.Store)
                .OnDelete(DeleteBehavior.NoAction);*/
        }
    }
}