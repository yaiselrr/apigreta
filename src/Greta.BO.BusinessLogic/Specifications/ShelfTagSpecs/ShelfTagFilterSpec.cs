using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.ShelfTagSpecs;

/// <summary>
/// ShelfTag Filter Specification
/// </summary>
public sealed class ShelfTagFilterSpec: Specification<ShelfTag>
{
    /// <inheritdoc />
    public ShelfTagFilterSpec(ShelfTagSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.ProductName, $"%{filter.Search}%");

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<ShelfTag>>)OrderExpressions).Add(new OrderExpressionInfo<ShelfTag>(
            splited[0] switch
            {
                _ => f => f.ProductName
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}