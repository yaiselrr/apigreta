using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategoryGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}