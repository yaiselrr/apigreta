using Greta.Sdk.ExternalScale.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

public class ExternalScaleSendDataToScaleRequest
{
    [FromRoute(Name = "store")] public long Store { get; set; }
    [FromRoute(Name = "department")] public long Department { get; set; }
    [FromRoute(Name = "type")] public ExternalScaleOperationType Type { get; set; }
    [FromRoute(Name = "partial")] public bool Partial { get; set; }
}