using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.OnlineStoreSpec;


/// <inheritdoc/>
public sealed class OnlineStoreGetOnlineCategoryForStoreSpec : Specification<OnlineCategory>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="onlineStoreId"></param>
    public OnlineStoreGetOnlineCategoryForStoreSpec(long categoryId, long onlineStoreId)
    {
        Query.Where(x => x.CategoryId == categoryId && x.OnlineStoreId == onlineStoreId);
    }
}