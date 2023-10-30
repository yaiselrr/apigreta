using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Scalendar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScalendarEndpoints;
[Route("api/Scalendar")]
public class ScalendarChangeState : EndpointBaseAsync.WithRequest<ScalendarChangeStateRequest>.WithActionResult<ScalendarChangeStateResponse>
{
    private readonly IMediator _mediator;

    public ScalendarChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the Scalendar entity",
        Description = "Change the state of the Scalendar entity",
        OperationId = "Scalendar.ChangeState",
        Tags = new[] { "Scalendar" })
    ]
    [ProducesResponseType(typeof(ScalendarChangeStateResponse), 200)]
    public override async Task<ActionResult<ScalendarChangeStateResponse>> HandleAsync(
        [FromRoute] ScalendarChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new ScalendarChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}