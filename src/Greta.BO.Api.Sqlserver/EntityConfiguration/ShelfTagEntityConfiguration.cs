using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.EntityConfiguration
{
    public class ShelfTagEntityConfiguration : IEntityTypeConfiguration<ShelfTag>
    {
        public void Configure(EntityTypeBuilder<ShelfTag> builder)
        {
            builder.ConfigurationBase<long, string, ShelfTag>();

            builder.Property(x => x.Price).HasColumnType("numeric(18,2)");
        }
    }
}