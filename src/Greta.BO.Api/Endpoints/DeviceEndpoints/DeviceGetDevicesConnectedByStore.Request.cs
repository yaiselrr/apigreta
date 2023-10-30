using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

public class DeviceGetDevicesConnectedByStoreRequest
{
    [FromRoute(Name = "storeId")]public int StoreId { get; set; }
}