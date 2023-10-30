using System;
using Greta.Sdk.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greta.BO.Api.Sqlserver.Extensions
{
    public static class CommonExtensions
    {
        static CommonExtensions()
        {
        }

        /// <summary>
        ///     Sets the traversal properties of an entity that implements the IEntityBase interface
        /// </summary>
        /// <typeparam name="TKey">Type of data that will identify the record</typeparam>
        /// <typeparam name="TUserKey">Type of data that the user will identify</typeparam>
        /// <typeparam name="TEntity">The entity type to be configured.</typeparam>
        /// <param name="userRequired"></param>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public static void ConfigurationLocalizationBase<TKey, TUserKey, TEntity>(
            this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IEntityLocationBase<TKey, TUserKey>
        {
            builder.Property(x => x.CountryName).HasColumnType("varchar(30)");
            builder.Property(x => x.CityName).HasColumnType("varchar(30)");
            builder.Property(x => x.ProvinceName).HasColumnType("varchar(30)");
            builder.Property(x => x.Zip).HasColumnType("varchar(12)");
        }
        
        private static DateTime FromCodeToData(DateTime fromCode, string name)
            => fromCode.Kind == DateTimeKind.Utc ? fromCode : throw new InvalidOperationException($"Column {name} only accepts UTC date-time values");

        private static DateTime FromDataToCode(DateTime fromData) 
            => fromData.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(fromData, DateTimeKind.Utc) : fromData.ToUniversalTime();

        public static PropertyBuilder<DateTime?> UsesUtc(this PropertyBuilder<DateTime?> property)
        {
            var name = property.Metadata.Name;
            return property.HasConversion<DateTime?>(
                fromCode => fromCode != null ? FromCodeToData(fromCode.Value, name) : default,
                fromData => fromData != null ? FromDataToCode(fromData.Value) : default
            );
        }

        public static PropertyBuilder<DateTime> UsesUtc(this PropertyBuilder<DateTime> property)
        {
            var name = property.Metadata.Name;
            return property.HasConversion(fromCode => FromCodeToData(fromCode, name), fromData => FromDataToCode(fromData));
        }
    }
}