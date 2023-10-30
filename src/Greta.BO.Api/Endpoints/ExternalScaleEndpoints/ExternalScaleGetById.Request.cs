using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

public class ExternalScaleGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}