using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.ReportSpecs;

/// <summary>
/// Report Filter Specification
/// </summary>
public sealed class ReportFilterSpec: Specification<Api.Entities.Report>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    public ReportFilterSpec(ReportSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Name, $"%{filter.Search}%");
       
        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));

        Query.Where(c => filter.Category == ReportCategory.CORPORATES && c.Category == filter.Category || filter.Category != ReportCategory.CORPORATES);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Api.Entities.Report>>)OrderExpressions).Add(new OrderExpressionInfo<Api.Entities.Report>(
            splited[0] switch
            {
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}