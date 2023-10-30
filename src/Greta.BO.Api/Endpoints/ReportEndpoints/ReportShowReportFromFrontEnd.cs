using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;

[Route("api/Report")]
public class ReportShowReportFromFrontEnd: EndpointBaseAsync.WithRequest<ReportShowReportFromFrontEndRequest>.WithResult<RedirectResult>
{
    private readonly IConfiguration _configuration;

    public ReportShowReportFromFrontEnd(IConfiguration configuration)
    {       
        _configuration = configuration;
    }

    [HttpGet("ShowSingleLongParameterReport/{reportName}/{parameterLongId}")]
    [SwaggerOperation(
        Summary = "Show Report from Front End",
        Description = "Show Report from Front End",
        OperationId = "Report.FromFrontEndPoint",
        Tags = new[] { "Report" })
    ]
    [ProducesResponseType(308)]
    [ProducesResponseType(404)]
    public override async Task<RedirectResult> HandleAsync([FromMultiSource] ReportShowReportFromFrontEndRequest request, CancellationToken cancellationToken = default)
    {
        var reportUrlBase = _configuration["Enterprise:ReportUrl"];
        if (reportUrlBase != null && !reportUrlBase.EndsWith("/"))
        {
            reportUrlBase += "/";
        }

        var clientCode = _configuration["Company:CompanyCode"];
        var url = $@"{reportUrlBase}CustomReport/ShowSingleLongParameterReport/{clientCode}/{request.ReportName}/{request.ParameterLongId}?token={request.Token}";

        return await Task.FromResult(RedirectPreserveMethod(url));
    }    
}