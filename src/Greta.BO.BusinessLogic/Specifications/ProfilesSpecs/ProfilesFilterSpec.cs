using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class ProfilesFilterSpec:Specification<Profiles>
{
/// <summary>
/// 
/// </summary>
/// <param name="filter"></param>
    public ProfilesFilterSpec(ProfilesSearchModel filter)
    {
        Query.Include(x => x.Application);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Name, $"%{filter.Search}%");
            Query.Search(c => c.Application.Name, $"%{filter.Search}%");
        }
        
        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));
        
        if (filter.ApplicationId > 0)
            Query.Where(c => c.ApplicationId == filter.ApplicationId);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Profiles>>)OrderExpressions).Add(new OrderExpressionInfo<Profiles>(
            splited[0] switch
            {
                "application" => f => f.Application.Name,
                _ => f => f.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));
    }
}
