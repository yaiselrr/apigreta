using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;

public class ReportShowReportRequest
{
    [FromRoute(Name = "reportId")]public int Id { get; set; }   
    [FromRoute(Name = "parameters")] public string Parameters { get; set; }  
    [FromQuery(Name = "token")] public string Token { get; set; }
}