using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;
[Route("api/ScaleCategory")]
public class ScaleCategoryChangeState : EndpointBaseAsync.WithRequest<ScaleCategoryChangeStateRequest>.WithActionResult<ScaleCategoryChangeStateResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the scale category entity",
        Description = "Change the state of the scale category entity",
        OperationId = "ScaleCategory.ChangeState",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryChangeStateResponse), 200)]
    public override async Task<ActionResult<ScaleCategoryChangeStateResponse>> HandleAsync(
        [FromRoute] ScaleCategoryChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new ScaleCategoryChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}