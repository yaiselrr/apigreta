using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;

public class ReportGetByIdRequest
{
    [FromRoute(Name = "reportId")]public int Id { get; set; }
}