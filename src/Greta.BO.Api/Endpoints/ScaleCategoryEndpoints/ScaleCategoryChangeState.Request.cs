using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;

public class ScaleCategoryChangeStateRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromRoute(Name = "state")] public bool State { get; set; }
}