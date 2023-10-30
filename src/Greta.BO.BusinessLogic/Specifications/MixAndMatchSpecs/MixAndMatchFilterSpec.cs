using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Specifications.MixAndMatchSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class MixAndMatchFilterSpec:Specification<MixAndMatch>
{
/// <summary>
/// 
/// </summary>
/// <param name="filter"></param>
    public MixAndMatchFilterSpec(MixAndMatchSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Name, $"%{filter.Search}%");        

        if (!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));

        if (filter.MixAndMatchType >= 0)        
            Query.Where(c => c.MixAndMatchType == filter.MixAndMatchType);
       
        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<MixAndMatch>>)OrderExpressions).Add(new OrderExpressionInfo<MixAndMatch>(
            splited[0] switch
            {                
                _ => f => f.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));
    }
}
