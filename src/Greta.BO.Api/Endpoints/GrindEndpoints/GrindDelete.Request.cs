using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;

public class GrindDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}