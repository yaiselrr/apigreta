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
public class WorkTimeReport : EndpointBaseAsync.WithRequest<WorkTimeReportRequest>.WithActionResult<TimeKeepingWorkTimeReportResponse>
{
    private readonly IMediator _mediator;

    public WorkTimeReport(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("WorkTimeReport/{storeId}")]
    [SwaggerOperation(
        Summary = "Gets list of time worked by employee by store",
        Description = "Gets list of time worked by employee by store",
        OperationId = "TimeKeeping.WorkTimeReport",
        Tags = new[] { "TimeKeeping" })
    ]
    [ProducesResponseType(typeof(TimeKeepingWorkTimeReportResponse), 200)]
    public override async Task<ActionResult<TimeKeepingWorkTimeReportResponse>> HandleAsync(
        [FromMultiSource] WorkTimeReportRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new TimeKeepingWorkTimeReportQuery(request.StoreId, request.Filter), cancellationToken);
        
    }
}