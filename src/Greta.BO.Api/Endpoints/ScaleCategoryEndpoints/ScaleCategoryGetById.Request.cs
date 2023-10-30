using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;

public class ScaleCategoryGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}