using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;
[Route("api/Family")]
public class ChangeState : EndpointBaseAsync.WithRequest<ChangeStateRequest>.WithActionResult<FamilyChangeStateResponse>
{
    private readonly IMediator _mediator;

    public ChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of entity",
        Description = "Change State of entity",
        OperationId = "Family.ChangeState",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyChangeStateResponse), 200)]
    public override async Task<ActionResult<FamilyChangeStateResponse>> HandleAsync(
        [FromRoute] ChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new FamilyChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}