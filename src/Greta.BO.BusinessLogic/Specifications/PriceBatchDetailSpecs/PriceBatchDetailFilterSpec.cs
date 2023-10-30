using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// PriceBatchDetail Filter Specification
/// </summary>
public sealed class PriceBatchDetailFilterSpec : Specification<PriceBatchDetail>
{
    /// <inheritdoc />
    public PriceBatchDetailFilterSpec(PriceBatchDetailSearchModel filter)
    {
        Query.Include(x => x.Family);
        Query.Include(x => x.Category);
        Query.Include(x => x.Product);

        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Product.Name, $"%{filter.Search}%");
        if (filter.ProductId > 0)
            Query.Where(c => c.ProductId == filter.ProductId);
        if (filter.HeaderId > 0)
            Query.Where(c => c.HeaderId == filter.HeaderId);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<PriceBatchDetail>>)OrderExpressions).Add(new OrderExpressionInfo<PriceBatchDetail>(
            splited[0] switch
            {
                "productName" => f => f.Product.Name,
                "upc" => f => f.Product.UPC,
                "familyName" => f => f.Family.Name,
                "price" => f => f.Price,
                _ => f => f.Product.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));

    }
}