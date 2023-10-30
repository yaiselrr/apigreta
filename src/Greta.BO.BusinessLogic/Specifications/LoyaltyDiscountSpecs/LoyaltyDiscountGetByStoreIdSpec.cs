using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.LoyaltyDiscountSpecs;

/// <summary>
/// LoyaltyDiscount Filter Specification
/// </summary>
public sealed class LoyaltyDiscountGetByStoreIdSpec: Specification<LoyaltyDiscount>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="storeId"></param>
    /// <param name="id"></param>
    public LoyaltyDiscountGetByStoreIdSpec(long storeId, long? id)
    {
        if (id == null || id == 0)
        {
            Query.Where(c => c.StoreId == storeId);
        }
        else
        {
            Query.Where(c => c.StoreId == storeId && c.Id != id);
        }
    }
}