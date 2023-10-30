using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

public class ExternalScaleDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}