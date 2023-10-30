using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.VendorOrderSpecs;

/// <summary>
/// Vendor Order Filter Specification
/// </summary>
public sealed class VendorOrderFilterSpec : Specification<VendorOrder>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    public VendorOrderFilterSpec(VendorOrderSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.InvoiceNumber, $"%{filter.Search}%");

        if (!string.IsNullOrEmpty(filter.VendorName))
            Query.Where(c => c.Vendor.Name.Contains(filter.VendorName));
        if (!string.IsNullOrEmpty(filter.StoreName))
            Query.Where(c => c.Store.Name.Contains(filter.StoreName));
        if (!string.IsNullOrEmpty(filter.InvoiceNumber))
            Query.Where(c => c.InvoiceNumber.Contains(filter.InvoiceNumber));
        if (filter.VendorId > 0)
            Query.Where(c => c.VendorId == filter.VendorId);
        if (filter.StoreId > 0)
            Query.Where(c => c.StoreId == filter.StoreId);
        if (filter.Status != null)
            Query.Where(c => c.Status == filter.Status.Value);
        if (filter.IsDsd)
            Query.Where(c => c.IsDirectStoreDelivery);

        Query.Include(x => x.User);
        Query.Include(x => x.Store);
        Query.Include(x => x.Vendor);
        
        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<VendorOrder>>)OrderExpressions).Add(new OrderExpressionInfo<VendorOrder>(
            splited[0] switch
            {
                "vendor" => f => f.Vendor.Name,
                _ => f => f.ReceivedDate
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));
    }
}