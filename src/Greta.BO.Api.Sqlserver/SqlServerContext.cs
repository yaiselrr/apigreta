using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Greta.Sdk.Core.Abstractions;
using Greta.Sdk.EFCore.Extensions;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Greta.BO.Api.Sqlserver
{
    public class SqlServerContext : DbContext
    {
        /// <summary>
        ///     Id for seed initial data
        /// </summary>
        public static readonly string SeedUserId = "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6";

        public static DateTime createdAt = new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        ///     Provide the information of the authenticated user during the request
        /// </summary>
        protected readonly IAuthenticateUser<string> AuthenticateUser;

        public SqlServerContext(DbContextOptions options, IAuthenticateUser<string> authenticatetUser) : base(options)
        {
            AuthenticateUser = authenticatetUser;
            
        }

        public SqlServerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.RegisterEntityConfigurations<SqlServerContext>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerContext).Assembly);
            
            modelBuilder.ApplyUtcDateTimeConverter();
        }

        //remove on production
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateDatesToUtc();
            
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IBase && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IBase<long, string>) entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    if (AuthenticateUser != null && AuthenticateUser.IsAuthenticated)
                        ((IBase<long, string>) entityEntry.Entity).UserCreatorId = AuthenticateUser?.UserId;
                    ((IBase<long, string>) entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
            // }
            // catch (DbEntityValidationException ex)
            // {

            // }
        }
        
        private void UpdateDatesToUtc()
        {
            if (!ChangeTracker.HasChanges()) return;

            var modifiedEntries = ChangeTracker.Entries().Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                entry.ModifyTypes<DateTime>(ConvertToUtc);
                entry.ModifyTypes<DateTime?>(ConvertToUtc);
            }
        }
        private static DateTime ConvertToUtc(DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Utc) return dt;
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc); //dt.ToUniversalTime();
        }

        private static DateTime? ConvertToUtc(DateTime? dt)
        {
            if (dt?.Kind == DateTimeKind.Utc) return dt;
            return dt == null ? null: DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);//dt?.ToUniversalTime();
        }
    }
  
    public static class TypeReflectionExtension
    {
        static Dictionary<Type, PropertyInfo[]> PropertyInfoCache = new Dictionary<Type, PropertyInfo[]>();

        static void TypeReflectionHelper()
        {
            PropertyInfoCache = new Dictionary<Type, PropertyInfo[]>();
        }

        public static PropertyInfo[] GetTypeProperties(this Type type)
        {
            if (!PropertyInfoCache.ContainsKey(type))
            {
                PropertyInfoCache[type] = type.GetProperties();
            }
            return PropertyInfoCache[type];
        }

        public static void ModifyTypes<T>(this EntityEntry dbEntityEntry, Func<T, T> method)
        {
            foreach (var propertyInfo in dbEntityEntry.Entity.GetType().GetTypeProperties().Where(p => p.PropertyType == typeof(T) && p.CanWrite))
            {
                propertyInfo.SetValue(dbEntityEntry.Entity, method(dbEntityEntry.CurrentValues.GetValue<T>(propertyInfo.Name)));
            }
        }
    }
}