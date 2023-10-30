using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface IOnlineStoreRepository : IOperationBase<long, string, OnlineStore>
    {
    }
    
    public interface IOnlineCategoryRepository : IOperationBase<long, string, OnlineCategory>
    {
    }
    
    public interface IOnlineProductRepository : IOperationBase<long, string, OnlineProduct>
    {
    }
}