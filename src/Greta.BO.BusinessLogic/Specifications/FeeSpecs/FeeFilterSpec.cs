using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.FeeSpecs;

/// <summary>
/// Fee Filter Specification
/// </summary>
public sealed class FeeFilterSpec: Specification<Fee>
{
    public FeeFilterSpec(FeeSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Name, $"%{filter.Search}%");
       
        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Fee>>)OrderExpressions).Add(new OrderExpressionInfo<Fee>(
            splited[0] switch
            {
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}