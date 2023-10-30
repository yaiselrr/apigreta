using System;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TimeKeeping;

public class ClockInRequest
{ 
    [FromBody] public ClockInModel ClockInModel { get; set; }
}

public class ClockInModel
{
    
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string DeviceLicenceCode { get; set; }
    public DateTime Date { get; set; }
    public string FormatDate { get; set; }
}