using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategorySetTargetGrossProfitRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }
    [FromBody] 
    public CategoryTargetGrossProfitModel CategoryTargetGrossProfitDto { get; set; }
}