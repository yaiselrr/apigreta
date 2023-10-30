using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Animal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;
[Route("api/Animal")]
public class AnimalChangeState : EndpointBaseAsync.WithRequest<AnimalChangeStateRequest>.WithActionResult<AnimalChangeStateResponse>
{
    private readonly IMediator _mediator;

    public AnimalChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the Animal entity",
        Description = "Change the state of the Animal entity",
        OperationId = "Animal.ChangeState",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalChangeStateResponse), 200)]
    public override async Task<ActionResult<AnimalChangeStateResponse>> HandleAsync(
        [FromRoute] AnimalChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new AnimalChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}