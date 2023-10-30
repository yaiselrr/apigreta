using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.LoyaltyDiscountSpecs;

/// <summary>
/// LoyaltyDiscount Filter Specification
/// </summary>
public sealed class LoyaltyDiscountFilterSpec: Specification<LoyaltyDiscount>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    public LoyaltyDiscountFilterSpec(LoyaltyDiscountSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Name, $"%{filter.Search}%");
       
        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));

        Query.Include(x => x.Store);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<LoyaltyDiscount>>)OrderExpressions).Add(new OrderExpressionInfo<LoyaltyDiscount>(
            splited[0] switch
            {
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}