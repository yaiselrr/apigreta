#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CsvHelper;
using CsvHelper.TypeConversion;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Exporters;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Notifiers;
using Microsoft.Extensions.DependencyInjection;

namespace Greta.BO.BusinessLogic.Extensions
{
    /// <summary>
    ///  Simple extensions used mostly in <see cref="Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers.BaseImport{T}"/> class
    /// </summary>
    public static class BaseImportExtensions
    {
        /// <summary>
        /// Inyect All dependencies we need for the import export process
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void AddExportImportSupport(this IServiceCollection services)
        {
            services.AddSingleton<INotifier, SignalRNotifier>();
            services.AddScoped<ICategoryExport, CategoryExport>();
            services.AddScoped<IDepartmentExport, DepartmentExport>();
            services.AddScoped<IFamilyExport, FamilyExport>();
            services.AddScoped<IProductExport, ProductExport>();
            services.AddScoped<IScaleCategoryExport, ScaleCategoryExport>();
            services.AddScoped<IScaleProductExport, ScaleProductExport>();
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> with properties that match provided predicate. it process only those
        /// properties that are assignable from strings and are not classes. If and error occurs it returns
        /// and empty list and invoke the notifyError action if specified.
        /// </summary>
        /// <param name="type">type to extract properties from</param>
        /// <param name="where">predicate to filter properties</param>
        /// <param name="notifyError"><see cref="Action{Exception}"/>to notify an error</param>
        public static List<string> GetBaseProperties<T>(this T type, Expression<Func<PropertyInfo, bool>> @where,
            Action<Exception>? notifyError = null)
        {
            try
            {
                return type!.GetType()
                    .GetProperties(BindingFlags.Public)
                    .AsQueryable()
                    .Where(x =>
                        // x.PropertyType.IsAssignableFrom(typeof(string)) ||
                        !x.PropertyType.IsClass
                    )
                    .ToWhere(@where)
                    .Map(x => x.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                if (notifyError != null)
                    notifyError.Invoke(ex);
                return new();
            }
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> with properties that are considered as base properties, this means that 
        /// are not UpdateAt, CreatedAt, UserCreatorId, State and if specified Id. it process only those
        /// properties that are assignable from strings and are not classes.
        /// </summary>
        /// <param name="type">type to extract properties from</param>
        /// <param name="includeId">if true it will include the Id property on the properties list</param>
        /// <param name="notifyError">action to notify an error</param>
        public static List<string> GetBaseProperties<T>(this T obj, bool includeId = true,
            Action<Exception>? notifyError = null)
        {
            try
            {
                if (includeId)
                {
                    return obj.GetType()
                        .GetProperties()
                        //.AsQueryable()
                        // .Where(x =>
                        //     // x.PropertyType.IsAssignableFrom(typeof(string)) ||
                        //     !x.PropertyType.IsClass
                        // )
                        .Where(x => (!x.PropertyType.IsClass || x.PropertyType.IsAssignableFrom(typeof(string))) &&
                                    x.Name != "UpdatedAt" && x.Name != "CreatedAt" && x.Name != "UserCreatorId" &&
                                    x.Name != "State")
                        .Map(x => x.Name)
                        .ToList();
                }

                return obj!.GetType()
                    .GetProperties()
                    //.AsQueryable()
                    // .Where(x =>
                    //     // x.PropertyType.IsAssignableFrom(typeof(string)) ||
                    //     !x.PropertyType.IsClass
                    // )
                    .Where(x => (!x.PropertyType.IsClass || x.PropertyType.IsAssignableFrom(typeof(string))) &&
                                x.Name != "Id" &&
                                x.Name != "UpdatedAt" && x.Name != "CreatedAt" && x.Name != "UserCreatorId" &&
                                x.Name != "State")
                    .Map(x => x.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                if (notifyError != null)
                    notifyError(ex);
                return new();
            }
        }

        /// <summary>
        ///  returns a CsvValue the mapping <see cref="Dictionary{string, string}"/> with the mapping structure, to map
        ///  the value it search for a field in the csv reading map and returns the Key
        /// </summary>
        /// <typeparam name="TReturn">type to return</typeparam>
        /// <param name="mapping">mapping <see cref="Dictionary{string, string}"/> with the data</param>
        /// <param name="csv"><see cref="CsvReader"/>with data</param>
        /// <param name="field">field to search for</param>
        /// <param name="defaultReturn">default value to return</param>
        /// <param name="notifyError">notify error <see cref="Action{Exception}"/> to handle error</param>
        public static TReturn GetCsValue<TReturn>(this Dictionary<string, string> mapping, CsvReader csv, string field,
            TReturn defaultReturn,
            Action<Exception>? notifyError = null)
        {
            try
            {
                return !mapping.ContainsValue(field)
                    ? defaultReturn
                    : csv.GetField<TReturn>(mapping.FirstOrDefault(x => x.Value == field).Key);
            }
            catch (Exception ex)
            {
                if (notifyError != null)
                    notifyError.Invoke(ex);
                return defaultReturn;
            }
        }

        /// <summary>
        ///  returns a CsvValue the mapping <see cref="Dictionary{string, string}"/> with the mapping structure, to map
        ///  the value it search for a field in the csv reading map and returns the Key
        /// </summary>
        /// <typeparam name="TReturn">type to return</typeparam>
        /// <typeparam name="TConverter">converter tu use to cast result</typeparam>
        /// <param name="mapping">mapping <see cref="Dictionary{string, string}"/> with the data</param>
        /// <param name="csv"><see cref="CsvReader"/>with data</param>
        /// <param name="field">field to search for</param>
        /// <param name="defaultReturn">default value to return</param>
        /// <param name="notifyError">notify error <see cref="Action{Exception}"/> to handle error</param>
        public static TReturn GetCsValue<TReturn, TConverter>(this Dictionary<string, string> mapping, CsvReader csv,
            string field, TReturn defaultReturn, Action<Exception>? notifyError = null)
            where TConverter : ITypeConverter
        {
            try
            {
                return !mapping.ContainsValue(field)
                    ? defaultReturn
                    : mapping.First<KeyValuePair<string, string>>().Key != null
                        ? csv.GetField<TReturn, TConverter>(mapping.FirstOrDefault(x => x.Value == field).Key)
                        : defaultReturn;
            }
            catch (Exception ex)
            {
                if (notifyError != null)
                    notifyError.Invoke(ex);
                return defaultReturn;
            }
        }

        /// <summary>
        ///  returns a CsvValue the mapping <see cref="Dictionary{string, string}"/> with the mapping structure, to map
        ///  the value it search for a field in the csv reading map and returns the Key
        /// </summary>
        /// <param name="mapping">mapping <see cref="Dictionary{string, string}"/> with the data</param>
        /// <param name="csv"><see cref="CsvReader"/>with data</param>
        /// <param name="type">type to process</typeparam>
        /// <param name="field">field to search for</param>
        /// <param name="defaultReturn">default value to return</param>
        /// <param name="notifyError">notify error <see cref="Action{Exception}"/> to handle error</param>
        public static object GetCsValue(this Dictionary<string, string> mapping, CsvReader csv, Type type,
            string field, object defaultReturn, Action<Exception>? notifyError = null)
        {
            try
            {
                return !mapping.ContainsValue(field)
                    ? defaultReturn
                    : csv.GetField(type, mapping.FirstOrDefault(x => x.Value == field).Key);
            }
            catch (Exception ex)
            {
                if (notifyError != null)
                    notifyError.Invoke(ex);
                return defaultReturn;
            }
        }

        /// <summary>
        /// returns a <see cref="List{string}"/> with all column names, exclude the Id column
        /// </summary>
        public static List<string> GetColumnName(this object @return, Action<Exception>? notifyError = null)
        {
            try
            {
                var modelHeaders = new List<string>();
                modelHeaders.AddRange(@return.GetBaseProperties(includeId: false, notifyError: notifyError));
                return modelHeaders;
            }
            catch (Exception ex)
            {
                if (notifyError != null)
                    notifyError.Invoke(ex);
                return new();
            }
        }

        /// <summary>
        /// returns a <see cref="List{string}"/> with all columns names of the type. include the Id column
        /// </summary>
        public static List<string> GetAllColumnName(this object @return, Action<Exception>? notifyError = null)
        {
            try
            {
                var modelHeaders = new List<string>();
                modelHeaders.AddRange(@return.GetBaseProperties(includeId: true, notifyError: notifyError));
                return modelHeaders;
            }
            catch (Exception ex)
            {
                if (notifyError != null)
                    notifyError.Invoke(ex);
                return new();
            }
        }

        public static List<T> MapToEntityList<T>(this Dictionary<string, string> mapping, CsvReader csv,
            List<long> storeIds,
            Action<HandlerMessage>? notify = null)
            where T : new()
        {
            List<T> result = new();
            try
            {
                var idx = 0;
                notify?.Invoke(HandlerMessage.New($"Mapping property map into list of Entities {nameof(T)}"));
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    idx++;
                    var entity = new T();
                    var properties = entity.GetBaseProperties();
                    entity.GetType().GetProperty("State")?.SetValue(entity, true, null);
                    foreach (var prop in properties)
                    {
                        if (entity.GetType().GetProperty(prop)!.PropertyType.IsAssignableFrom(typeof(bool)))
                        {
                            var val = mapping.GetCsValue<bool, BooleanConverter>(csv, prop, false);
                            entity.GetType().GetProperty(prop)?.SetValue(entity, val, null);
                        }
                        else
                        {
                            var val = mapping.GetCsValue(csv, entity.GetType().GetProperty(prop)?.PropertyType, prop,
                                null);
                            entity.GetType().GetProperty(prop)?.SetValue(entity, val, null);
                        }
                    }

                    result.Add(entity);
                }

                notify?.Invoke(HandlerMessage.New($"Mapping completed, {idx} rows where processed"));
                return result;
            }
            catch (Exception ex)
            {
                notify.Invoke(HandlerMessage.New(nameof(T), ex));
                return new();
            }
        }

        private static IQueryable<T> ToWhere<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> @where = null!)
            where T : class
        {
            if (queryable != default)
            {
                return queryable.Where(@where);
            }

            return queryable!;
        }
    }
}