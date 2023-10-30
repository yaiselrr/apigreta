using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;

public class DiscountCreateRequest
{
    [FromBody] 
    public DiscountModel EntityDto { get; set; }
}