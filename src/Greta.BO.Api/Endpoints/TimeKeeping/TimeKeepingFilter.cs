using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.TimeKeepingQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TimeKeeping;
[Route("api/TimeKeeping")]
public class TimeKeepingFilter : EndpointBaseAsync.WithRequest<TimeKeepingFilterRequest>.WithActionResult<
    TimeKeepingFilterResponse>
{
    private readonly IMediator _mediator;

    public TimeKeepingFilter(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of time keeping entity",
        Description = "Gets a paginated list of time keeping entity",
        OperationId = "TimeKeeping.Filter",
        Tags = new[] { "TimeKeeping" })
    ]
    [ProducesResponseType(typeof(TimeKeepingFilterResponse), 200)]
    public override async Task<ActionResult<TimeKeepingFilterResponse>> HandleAsync(
        [FromMultiSource] TimeKeepingFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new TimeKeepingFilterQuery(request.CurrentPage, request.PageSize, request.Filter),
            cancellationToken);
    }
}