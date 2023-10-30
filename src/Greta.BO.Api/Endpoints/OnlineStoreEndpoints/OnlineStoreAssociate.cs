using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;
[Route("api/OnlineStore")]
public class OnlineStoreAssociate : EndpointBaseAsync.WithRequest<OnlineStoreAssociateRequest>.WithActionResult<OnlineStoreAssociateResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreAssociate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("OnlineStoreAssociate/{entityId}/{token}/{isImport}")]
    [SwaggerOperation(
        Summary = "Associate the Online Store entity",
        Description = "Associate the Online Store entity",
        OperationId = "OnlineStore.OnlineStoreAssociate",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreAssociateResponse), 200)]
    public override async Task<ActionResult<OnlineStoreAssociateResponse>> HandleAsync(
        [FromRoute] OnlineStoreAssociateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new OnlineStoreAssociateCommand(request.Id, request.Token, request.IsImport), cancellationToken);
    }
}