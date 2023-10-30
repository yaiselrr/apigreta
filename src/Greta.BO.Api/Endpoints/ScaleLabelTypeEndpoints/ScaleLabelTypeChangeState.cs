using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;
[Route("api/ScaleLabelType")]
public class ScaleLabelTypeChangeState : EndpointBaseAsync.WithRequest<ScaleLabelTypeChangeStateRequest>.WithActionResult<ScaleLabelTypeChangeStateResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the scale label type entity",
        Description = "Change the state of the scale label type entity",
        OperationId = "ScaleLabelType.ChangeState",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeChangeStateResponse), 200)]
    public override async Task<ActionResult<ScaleLabelTypeChangeStateResponse>> HandleAsync(
        [FromRoute] ScaleLabelTypeChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new ScaleLabelTypeChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}