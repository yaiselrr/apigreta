#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Greta.Sdk.Core.Abstractions;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers
{
    public interface IModelImporter<T> : IIntrospector<T> 
        where T: class, IBase
    {
        Task Process(Dictionary<string, string> mapping, CsvReader csv);
    }

    public abstract class BaseImport<T> : Introspector<T>
        where T: class, IBase
    {
        protected readonly IServiceProvider Provider;
        private readonly ILogger _logger;

        protected BaseImport(ILogger logger, IServiceProvider provider, ILogger inLogger, INotifier notifier) :
            base(inLogger, nameof(T), notifier)
        {
            _logger = logger;
            Provider = provider;
        }

        public abstract Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds);
        
        // protected List<string> GetBaseProperties<TType>()
        // {
        //     return typeof(TType)
        //         .GetProperties()
        //         .Where(x =>
        //             x.PropertyType.IsAssignableFrom(typeof(string))
        //             || !x.PropertyType.IsClass
        //         )
        //         .Where(x => x.Name != "UpdatedAt" && x.Name != "CreatedAt" && x.Name != "UserCreatorId" &&
        //                     x.Name != "Id" && x.Name != "State")
        //         .Map(x => x.Name).ToList();
        // }
        // protected List<string> GetAllBaseProperties<TType>()
        // {
        //     return typeof(TType)
        //         .GetProperties()
        //         .Where(x =>
        //             x.PropertyType.IsAssignableFrom(typeof(string))
        //             || !x.PropertyType.IsClass
        //         )
        //          .Where(x => x.Name != "UpdatedAt" && x.Name != "CreatedAt" && x.Name != "UserCreatorId" && x.Name != "State")
        //         .Map(x => x.Name).ToList();
        // }
        // protected List<String> GetColumnName<TResult>()
        // {
        //     var modelHeaders = new List<string>();
        //     modelHeaders.AddRange(GetBaseProperties<TResult>());
        //     return modelHeaders;
        // }
        // protected List<String> GetAllColumnName<TResult>()
        // {
        //     var modelHeaders = new List<string>();
        //     modelHeaders.AddRange(GetAllBaseProperties<TResult>());
        //     return modelHeaders;
        // }
    }
}
