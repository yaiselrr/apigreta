using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.FamilySpecs;

/// <summary>
/// Inventory Filter Specification
/// </summary>
public sealed class InventoryFilterSpec: Specification<StoreProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="storeId"></param>
    public InventoryFilterSpec(InventorySearchModel filter, int currentPage,int pageSize, long storeId)
    {
        Query.Include(x => x.Product);
        Query.Include(x => x.BinLocation);        
        
        var t = Query.Where(x => x.StoreId == storeId && x.Product.State);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Where(c => c.Product.Name.Contains(filter.Search) ||
                             c.Product.UPC.Contains(filter.Search) ||
                             c.BinLocation.Name.Contains(filter.Search));
        }

        if (filter.DepartmentId > 0)
        {
            Query.Where(c => c.Product.DepartmentId == filter.DepartmentId);
        }

        if (filter.CategoryId > 0)
        {
            Query.Where(c => c.Product.CategoryId == filter.CategoryId);
        }

        if (filter.BinLocationId > 0)
        {
            Query.Where(c => c.BinLocationId == filter.BinLocationId);
        }

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<StoreProduct>>)OrderExpressions).Add(new OrderExpressionInfo<StoreProduct>(
            splited[0] switch
            {
                "upc" => p => p.Product.UPC,
                "product" => p => p.Product.Name,
                "binLocation" => b => b.BinLocation.Name,
                _ => p => p.Product.UPC                
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));

    }
}