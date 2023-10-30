using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;

public class LoyaltyDiscountFilterRequest
{
    [FromRoute(Name = "currentPage")]public int CurrentPage { get; set; }
    [FromRoute(Name = "pageSize")]public int PageSize { get; set; }
    [FromBody]public LoyaltyDiscountSearchModel Filter { get; set; }
}