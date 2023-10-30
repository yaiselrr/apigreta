using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;

public class ReportShowReportFileRequest
{    
    [FromRoute(Name = "reportId")]public int Id { get; set; }
    [FromRoute(Name = "format")] public string Format { get; set; }
    [FromRoute(Name = "parameters")] public string Parameters { get; set; }
    [FromRoute(Name = "inline")] public bool Inline { get; set; }
    [FromQuery(Name = "token")] public string Token { get; set; }
}