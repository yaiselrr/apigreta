using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;

public class LoyaltyDiscountUpdateRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromBody] public LoyaltyDiscountUpdateModel EntityDto { get; set; }
}