using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints
{
    public class CutListTemplateChangeStateRequest
    {
        [FromRoute(Name = "entityId")] public long Id { get; set; }
        [FromRoute(Name = "state")] public bool State { get; set; }
    }
}