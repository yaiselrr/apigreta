using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;

[Route("api/CutListTemplate")]
public class CutListTemplateChangeState:EndpointBaseAsync.WithRequest<CutListTemplateChangeStateRequest>.WithActionResult<CutListTemplateChangeStateResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of CutListTemplate",
        Description = "Change State of CutListTemplate",
        OperationId = "CutListTemplate.ChangeState",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateChangeStateResponse), 200)]
    public override async Task<ActionResult<CutListTemplateChangeStateResponse>> HandleAsync(
        [FromRoute] CutListTemplateChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {        
        return Ok(await _mediator.Send(new CutListTemplateChangeStateCommand(request.Id, request.State), cancellationToken));
    }
}
