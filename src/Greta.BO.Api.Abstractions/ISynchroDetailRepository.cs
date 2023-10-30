using System;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface ISynchroDetailRepository : IOperationBase<long, string, SynchroDetail>
    {
        Task<bool> CreateSynchroDetail<TData>(long storeId, TData data, SynchroType type, Func<TData, object> converter = null);
    }
}