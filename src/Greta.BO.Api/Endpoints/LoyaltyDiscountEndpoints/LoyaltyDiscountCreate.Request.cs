using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;

public class LoyaltyDiscountCreateRequest
{
    [FromBody] 
    public LoyaltyDiscountCreateModel EntityDto { get; set; }
}