using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.TimeKeepingQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TimeKeeping;
[Route("api/TimeKeeping")]
public class ExportCsvWorkTime : EndpointBaseAsync.WithRequest<ExportCsvWorkTimeRequest>.WithoutResult
{
    private readonly IMediator _mediator;

    public ExportCsvWorkTime(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ExportCsvWorkTime")]
    [SwaggerOperation(
        Summary = "Export Csv Work Time",
        Description = "Export Csv Work Time",
        OperationId = "TimeKeeping.ExportCsvWorkTime",
        Tags = new[] { "TimeKeeping" })
    ]
    [ProducesResponseType(typeof(ExportCsvWorkTimeResponse), 200)]
    public override async Task<ActionResult> HandleAsync(
        [FromBody] ExportCsvWorkTimeRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new ExportCsvWorkTimeQuery(request.Data), cancellationToken);
        if (result == null)
            return NotFound();
        
        var memoryStream = new MemoryStream();
        TextWriter tw = new StreamWriter(memoryStream);

        tw.WriteLine(result.Data);
        tw.Flush();
        tw.Close();

        return File(memoryStream.GetBuffer(), "text/plain", "mapp_work_time.csv");
    }
}