using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.OnlineStoreSpec;


/// <inheritdoc/>
public sealed class OnlineStoreGetOnlineCategorySpec : Specification<OnlineCategory>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryId"></param>
    public OnlineStoreGetOnlineCategorySpec(long categoryId)
    {
        Query.Where(x => x.CategoryId == categoryId);
    }
}