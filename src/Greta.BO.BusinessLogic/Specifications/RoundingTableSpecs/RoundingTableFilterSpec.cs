using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.RoundingTableSpecs;

/// <summary>
/// RoundingTable Filter Specification
/// </summary>
public sealed class RoundingTableFilterSpec: Specification<RoundingTable>
{
    public RoundingTableFilterSpec(RoundingTableSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.EndWith.ToString(), $"%{filter.Search}%");
       
        // if(!string.IsNullOrEmpty(filter.EndWith.ToString()))
        //     Query.Where(c => c.EndWith.ToString().Contains(filter.EndWith.ToString()));

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<RoundingTable>>)OrderExpressions).Add(new OrderExpressionInfo<RoundingTable>(
            splited[0] switch
            {
                _ => f => f.EndWith.ToString()
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}