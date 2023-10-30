using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Greta.BO.Api.Entities.Attributes;
using Greta.Sdk.Core.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Greta.BO.Api.Entities
{
    public class BaseEntityLong : IEntityLong<string>
    {
        public static IEnumerable<(PropertyInfo property, FieldInfoAttribute field)> GetFieldInto<T>()
            => typeof(T)
                .GetProperties()
                .Where(prop => prop.GetCustomAttribute<FieldInfoAttribute>() is not null)
                .Select(prop => (prop, prop.GetCustomAttribute<FieldInfoAttribute>()));
        
        #region Base

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        #endregion Base
    }
}