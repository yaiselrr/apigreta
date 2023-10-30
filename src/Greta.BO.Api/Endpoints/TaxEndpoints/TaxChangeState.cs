using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Tax;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;
[Route("api/Tax")]
public class TaxChangeState : EndpointBaseAsync.WithRequest<TaxChangeStateRequest>.WithActionResult<TaxChangeStateResponse>
{
    private readonly IMediator _mediator;

    public TaxChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of entity",
        Description = "Change State of entity",
        OperationId = "Tax.ChangeState",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxChangeStateResponse), 200)]
    public override async Task<ActionResult<TaxChangeStateResponse>> HandleAsync(
        [FromRoute] TaxChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new TaxChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}