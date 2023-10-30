using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.OnlineStoreSpecs;

/// <summary>
/// OnlineStore Filter Specification
/// </summary>
public sealed class OnlineStoreFilterSpec: Specification<OnlineStore>
{
    /// <inheritdoc />
    public OnlineStoreFilterSpec(OnlineStoreSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(x => x.Name, $"%{filter.Search}%");
       
        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(x => x.Name.Contains(filter.Name));
        
        Query.Where(x => !x.Isdeleted || !x.IsActiveWebSite);
        Query.Include(x => x.Store);
        Query.Include(x => x.Departments);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<OnlineStore>>)OrderExpressions).Add(new OrderExpressionInfo<OnlineStore>(
            splited[0] switch
            {
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}