using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.OnlineStoreSpec;


/// <inheritdoc/>
public sealed class OnlineStoreGetOnlineProductForStoreSpec : Specification<OnlineProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="onlineStoreId"></param>
    public OnlineStoreGetOnlineProductForStoreSpec(long productId, long onlineStoreId)
    {
        Query.Where(x => x.ProductId == productId && x.OnlineStoreId == onlineStoreId);
    }
}