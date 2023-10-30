using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

public class DeviceSendActionRequest
{
    [FromRoute(Name = "entityId")] public long EntityId { get; set; }
    [FromRoute(Name = "actionId")] public int ActionId { get; set; }
}