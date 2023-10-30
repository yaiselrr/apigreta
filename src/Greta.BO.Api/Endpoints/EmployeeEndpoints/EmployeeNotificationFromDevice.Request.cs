using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.EmployeeEndpoints;

public class EmployeeNotificationFromDeviceRequest
{
    [FromBody] public EmployeeNotificationFromDeviceFilter Filter { get; set; }
}

public class EmployeeNotificationFromDeviceFilter
{
    public string DeviceLicenceCode { get; set; }
    public long EmployeeId { get; set; }
}