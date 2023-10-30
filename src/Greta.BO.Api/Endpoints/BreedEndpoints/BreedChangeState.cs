using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Breed;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;
[Route("api/Breed")]
public class BreedChangeState : EndpointBaseAsync.WithRequest<BreedChangeStateRequest>.WithActionResult<BreedChangeStateResponse>
{
    private readonly IMediator _mediator;

    public BreedChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the Breed entity",
        Description = "Change the state of the Breed entity",
        OperationId = "Breed.ChangeState",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedChangeStateResponse), 200)]
    public override async Task<ActionResult<BreedChangeStateResponse>> HandleAsync(
        [FromRoute] BreedChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new BreedChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}