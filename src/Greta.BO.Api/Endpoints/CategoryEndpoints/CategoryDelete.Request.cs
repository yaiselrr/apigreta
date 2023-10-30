using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategoryDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}