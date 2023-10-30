using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.InventorySpecs;

/// <summary>
/// Get store product by id Specification
/// </summary>
public sealed class InventoryGetStoreProductByIdSpec: Specification<StoreProduct>
{
    public InventoryGetStoreProductByIdSpec(long storeproductId)
    {
        Query.Include(x => x.Product);
        Query.Include(x => x.Product.Category);
        Query.Include(x => x.Product.Department);
        Query.Include(x => x.Store);
        Query.Where(x => x.Id == storeproductId);               
    }
}