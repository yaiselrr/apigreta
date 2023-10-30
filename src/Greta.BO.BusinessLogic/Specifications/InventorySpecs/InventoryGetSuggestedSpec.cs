using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.InventorySpecs;

/// <summary>
/// Get suggested inventory filter Specification
/// </summary>
public sealed class InventoryGetSuggestedSpec: Specification<StoreProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="storeId"></param>
    /// <param name="vendorId"></param>
    public InventoryGetSuggestedSpec(InventorySearchModel filter, long storeId, long vendorId)
    {
        Query.Include(x => x.Product)
             .ThenInclude(x => x.VendorProducts)
             .Where(x => x.StoreId == storeId &&
                         x.OrderTrigger > 0 &&
                         x.QtyHand <= x.OrderTrigger &&
                         x.Product.VendorProducts.Any(vp => vp.VendorId == vendorId));
        
        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Where(c => c.Product.Name.Contains(filter.Search) || c.Product.UPC.Contains(filter.Search));
        }

        if (filter.DepartmentId > 0)
        {
            Query.Where(c => c.Product.DepartmentId == filter.DepartmentId);
        }

        if (filter.CategoryId > 0)
        {
            Query.Where(c => c.Product.CategoryId == filter.CategoryId);
        }        
    }
}