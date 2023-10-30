using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;

public class StoreChangeStateRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromRoute(Name = "state")] public bool State { get; set; }
}