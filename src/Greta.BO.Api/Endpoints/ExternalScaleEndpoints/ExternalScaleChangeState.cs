using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;
[Route("api/ExternalScale")]
public class ExternalScaleChangeState : EndpointBaseAsync.WithRequest<ExternalScaleChangeStateRequest>.WithActionResult<ExternalScaleChangeStateResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the external scale entity",
        Description = "Change the state of the external scale entity",
        OperationId = "ExternalScale.ChangeState",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleChangeStateResponse), 200)]
    public override async Task<ActionResult<ExternalScaleChangeStateResponse>> HandleAsync(
        [FromRoute] ExternalScaleChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new ExternalScaleChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}