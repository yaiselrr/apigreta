using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.CutListTemplateSpecs;

/// <summary>
/// CutListTemplate Filter Specification
/// </summary>
public sealed class CutListTemplateFilterSpec: Specification<CutListTemplate>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    public CutListTemplateFilterSpec(CutListTemplateSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Name, $"%{filter.Search}%");
       
        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<CutListTemplate>>)OrderExpressions).Add(new OrderExpressionInfo<CutListTemplate>(
            splited[0] switch
            {
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}