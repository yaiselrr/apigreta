using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;
[Route("api/Grind")]
public class GrindChangeState : EndpointBaseAsync.WithRequest<GrindChangeStateRequest>.WithActionResult<GrindChangeStateResponse>
{
    private readonly IMediator _mediator;

    public GrindChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the Grind entity",
        Description = "Change the state of the Grind entity",
        OperationId = "Grind.ChangeState",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindChangeStateResponse), 200)]
    public override async Task<ActionResult<GrindChangeStateResponse>> HandleAsync(
        [FromRoute] GrindChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new GrindChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}