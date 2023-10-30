#nullable enable
using System;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    /// And exporter is a base class that basically reads data from 
    /// the database entities and match them with a provided mapping 
    /// in order to generate the CSV file
    /// </summary>
    public interface IExporter<T> : IIntrospector<T>
        where T: class, IBase
    {
    }
}
