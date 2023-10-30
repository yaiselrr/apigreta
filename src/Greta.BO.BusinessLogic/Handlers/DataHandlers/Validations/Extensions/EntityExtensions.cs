using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations.Extensions
{
    public static class EntityExtensions
    {
        public static TSource IfNotMappedGetPropertyValue<TSource>(this TSource source, Dictionary<string, string> mapping,
            string property, object provider)
        {
            if(mapping.ContainsValue(property))
                return source.GetPropValue<TSource>(property);
                
            var value = provider.GetType().GetProperty(property)?.GetValue(provider);
            source.GetType().GetProperty(property)?.SetValue(source, value);
            return source;
        }

        public static object GetPropValue(this object obj, string name) {
            foreach (var part in name.Split('.')) {
                if (obj == null) { return null; }

                var type = obj.GetType();
                var info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static Product PopulateIfNotMapped(this Product product, Dictionary<string, string> mapping, Category category)
        {
            product.CategoryId = category.Id;
            if(product.MinimumAge == 0)
                product.MinimumAge = category.MinimumAge ?? 0;
            product.DefaulShelfTagId = category.DefaulShelfTagId;
            product.SnapEBT.IfNotMappedGetPropertyValue(mapping, nameof(Product.NoDiscountAllowed), category);
            product.SnapEBT.IfNotMappedGetPropertyValue(mapping, nameof(Product.SnapEBT), category);
            product.PrintShelfTag.IfNotMappedGetPropertyValue(mapping, nameof(Product.DefaulShelfTag), category);
            product.NoDiscountAllowed.IfNotMappedGetPropertyValue(mapping, nameof(Product.NoDiscountAllowed), category);
            product.PromptPriceAtPOS.IfNotMappedGetPropertyValue(mapping, nameof(Product.PromptPriceAtPOS), category);
            product.AddOnlineStore.IfNotMappedGetPropertyValue(mapping, nameof(Product.AddOnlineStore), category);
            product.AllowZeroStock.IfNotMappedGetPropertyValue(mapping, nameof(Product.AllowZeroStock), category);
            product.DisplayStockOnPosButton.IfNotMappedGetPropertyValue(mapping, nameof(Product.DisplayStockOnPosButton), category);
            product.Modifier.IfNotMappedGetPropertyValue(mapping, nameof(Product.Modifier), category);
            return product;
        }

        public static ScaleProduct PopulateIfNotMapped(this ScaleProduct source, Dictionary<string, string> mapping,
            Category category, long department, ScaleCategory scaleCategory)
        {
            source.DepartmentId = department;
            source.CategoryId = category.Id;
            source.ScaleCategoryId = scaleCategory.Id;

            if (source.MinimumAge == 0)
                source.MinimumAge = category.MinimumAge ?? 0;

            source.DefaulShelfTagId =
                category.DefaulShelfTagId;

            source.NoDiscountAllowed.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.NoDiscountAllowed), category);
            source.SnapEBT.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.SnapEBT), category);
            source.PrintShelfTag.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.PrintShelfTag), category);
            source.NoPriceOnShelfTag.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.NoPriceOnShelfTag), category);
            source.PromptPriceAtPOS.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.PromptPriceAtPOS),category);
            source.AddOnlineStore.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.AddOnlineStore), category);
            source.AllowZeroStock.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.AllowZeroStock), category);
            source.DisplayStockOnPosButton.IfNotMappedGetPropertyValue(mapping,nameof(ScaleProduct.DisplayStockOnPosButton), category);
            source.Modifier.IfNotMappedGetPropertyValue(mapping, nameof(ScaleProduct.Modifier), category);
            
            return source;
        }

        public static T GetPropValue<T>(this Object obj, String name) {
            var retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T) retval;
        }

        public static TSource GetOrAddFromCache<TSource>(this Dictionary<long, TSource> cache, long id, 
            Func< long, Task<TSource>> serviceHandler, Action<HandlerMessage> notify)
            where TSource : class, IBase
        {
            if (!cache.TryGetValue(id, out TSource result))
            {
                var entity = Task.Run(async () => await serviceHandler(id)).Result;
                if (entity != null)
                {
                    cache.Add(id, entity);
                    return entity;
                }

                var msg = HandlerMessage.New($"{typeof(TSource)} was not found with id {id}", level: MessageLevel.Error);
                notify.Invoke(msg);
            }

            return result;
        }

        public static long GetOrAddFromCache<TSource>(this Dictionary<long, long> cache, long id,
            Func< long, Task<TSource>> serviceHandler, Action<HandlerMessage> notify)
            where TSource : class, IBase
        {
            if (!cache.TryGetValue(id, out long entId))
            {
                var entity = Task.Run(async () => await serviceHandler(id)).Result;
                if (entity != null)
                {
                    var entityId = entity.GetPropValue<long>("Id");
                    cache.Add(id, entityId);
                    return entityId;
                }

                var msg = HandlerMessage.New($"{typeof(TSource)} was not found with id {id}", level: MessageLevel.Error);
                notify.Invoke(msg);
            }

            return entId;
        }

        public static HandlerMessage ToHandlerMessage<TSource>(this TSource source)
        {
            if (source != null && source is HandlerMessage)
                return source as HandlerMessage;
            return new HandlerMessage();
        }
    }
}