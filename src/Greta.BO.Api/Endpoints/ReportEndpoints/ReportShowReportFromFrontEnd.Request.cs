using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;

public class ReportShowReportFromFrontEndRequest
{
    
    [FromRoute(Name = "reportName")]public string ReportName { get; set; }   
    [FromRoute(Name = "parameterLongId")] public long ParameterLongId { get; set; }  
    [FromQuery(Name = "token")] public string Token { get; set; }
}