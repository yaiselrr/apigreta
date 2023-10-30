using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategoryUpdateRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }
    [FromRoute(Name = "changeAllProducts")]
    public bool changeAllProducts { get; set; }
    [FromBody] 
    public CategoryModel EntityDto { get; set; }
}