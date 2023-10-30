using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategoryCreateRequest
{
    [FromBody] 
    public CategoryModel EntityDto { get; set; }
}