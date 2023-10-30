using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;

public class ScaleLabelTypeChangeStateRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromRoute(Name = "state")] public bool State { get; set; }
}