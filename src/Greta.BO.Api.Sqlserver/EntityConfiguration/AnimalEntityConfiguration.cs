using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class AnimalEntityConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.ConfigurationBase<long, string, Animal>();

            builder.Property(x => x.Tag).IsRequired();
            builder.HasOne(x => x.Rancher).WithMany(x => x.Animals).HasForeignKey(x => x.RancherId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Breed).WithMany(x => x.Animals).HasForeignKey(x => x.BreedId).OnDelete(DeleteBehavior.NoAction);
            
            builder.Property(x => x.LiveWeight).HasColumnType("numeric(18,2)");
            builder.Property(x => x.RailWeight).HasColumnType("numeric(18,2)");
            builder.Property(x => x.SubPrimalWeight).HasColumnType("numeric(18,2)");
            builder.Property(x => x.CutWeight).HasColumnType("numeric(18,2)");
            
            builder.HasMany(x => x.Customers).WithMany(x => x.Animals);
            
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId).OnDelete(DeleteBehavior.NoAction);
            
            builder.Property(x => x.LiveWeight).HasColumnType("numeric(18,2)");
            builder.Property(x => x.RailWeight).HasColumnType("numeric(18,2)");
            builder.Property(x => x.SubPrimalWeight).HasColumnType("numeric(18,2)");
            builder.Property(x => x.CutWeight).HasColumnType("numeric(18,2)");
            
        }
    }
}