using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.CutListDetailSpecs;

/// <summary>
/// Cut List Detail Filter Specification
/// </summary>
public sealed class CutListDetailFilterSpec : Specification<CutListDetail>
{
    /// <inheritdoc />
    public CutListDetailFilterSpec(CutListDetailSearchModel filter)
    {
        if (filter.CutListId > 0)
            Query.Where(c => c.CutListId == filter.CutListId);

        Query.Include(c => c.Product);
        
        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<CutListDetail>>)OrderExpressions).Add(new OrderExpressionInfo<CutListDetail>(
            splited[0] switch
            {
                _ => f => f.ProductId
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));
    }
}