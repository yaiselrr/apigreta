using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.StoreSpecs;

/// <summary>
/// Store Filter Specification
/// </summary>
public sealed class StoreFilterSpec: Specification<Store, StoreListModel>
{
    /// <inheritdoc />
    public StoreFilterSpec(StoreSearchModel filter)
    {

        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Name, $"%{filter.Search}%");
        }
        Query.Where(x => x.IsDeleted == filter.IsDeleted);

        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));
        if(filter.RegionId > 0)
            Query.Where(c => c.RegionId == filter.RegionId);
        Query.Include(x => x.Region);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Store>>)OrderExpressions).Add(new OrderExpressionInfo<Store>(
            splited[0] switch
            {
                "name" => f => f.Name,
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

        Query.Select(x =>
            new StoreListModel(x.Id, x.Name, x.IsDeleted, x.State, new RegionForStoreModel(x.Region.Id, x.Region.Name)));

    }
}