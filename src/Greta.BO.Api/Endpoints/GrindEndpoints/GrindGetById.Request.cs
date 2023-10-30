using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;

public class GrindGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}