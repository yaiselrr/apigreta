using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TimeKeeping;

public class EmployeeNotificationRequest
{
    [FromRoute(Name = "storeId")] public long StoreId { get; set; }
    [FromRoute(Name = "employeeId")] public long EmployeeId { get; set; }
}