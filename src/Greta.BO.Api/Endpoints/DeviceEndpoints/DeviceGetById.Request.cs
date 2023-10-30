using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

public class DeviceGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}