using System;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TimeKeeping;

public class ClockOutRequest
{ 
    [FromBody] public ClockOutModel ClockOutModel { get; set; }
}

public class ClockOutModel
{
    
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string DeviceLicenceCode { get; set; }
    public DateTime Date { get; set; }
    public string FormatDate { get; set; }
}