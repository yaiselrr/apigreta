using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.StoreProductSpecs;

/// <summary>
/// Get StoreProducts by Category
/// </summary>
public sealed class StoreProductsByCategoryAndStoreSpec : Specification<StoreProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="allStores"></param>
    /// <param name="regionId"></param>
    /// <param name="storeId"></param>
    public StoreProductsByCategoryAndStoreSpec(long categoryId, bool allStores, long regionId, long storeId)
    {
        Query.Include(sp => sp.Product )
            .Where(sp => sp.Product.State == true && 
                         sp.Product.CategoryId == categoryId);

        if (!allStores)
        {
            if (regionId > 0)
            {
                Query.Include(sp => sp.Store).Where(sp => sp.Store.RegionId == regionId);
            }
            else if (storeId > 0)
            {
                Query.Where(sp => sp.StoreId == storeId);
            }
        }
    }
}