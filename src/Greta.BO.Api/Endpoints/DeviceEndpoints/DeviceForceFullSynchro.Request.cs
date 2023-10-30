using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

public class DeviceForceFullSynchroRequest
{
    [FromRoute(Name = "deviceId")]public int DeviceId { get; set; }
}