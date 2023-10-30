using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.ScalendarSpecs;

/// <summary>
/// Scalendar Filter Specification
/// </summary>
public sealed class ScalendarFilterSpec: Specification<Scalendar>
{
    /// <inheritdoc />
    public ScalendarFilterSpec(ScalendarSearchModel filter)
    {
        Query.Include(x => x.Breeds);

        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Day, $"%{filter.Search}%");
       
        if(!string.IsNullOrEmpty(filter.Day))
            Query.Where(c => c.Day.Contains(filter.Day));

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Scalendar>>)OrderExpressions).Add(new OrderExpressionInfo<Scalendar>(
            splited[0] switch
            {
                _ => f => f.Id
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}