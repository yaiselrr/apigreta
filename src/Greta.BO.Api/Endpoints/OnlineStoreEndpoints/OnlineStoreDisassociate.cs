using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;
[Route("api/OnlineStore")]
public class OnlineStoreDisassociate : EndpointBaseAsync.WithRequest<OnlineStoreDisassociateRequest>.WithActionResult<OnlineStoreDisassociateResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreDisassociate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("OnlineStoreDisassociate")]
    [SwaggerOperation(
        Summary = "Disassociate the Online Store entity",
        Description = "Disassociate the Online Store entity",
        OperationId = "OnlineStore.OnlineStoreDisassociate",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreDisassociateResponse), 200)]
    public override async Task<ActionResult<OnlineStoreDisassociateResponse>> HandleAsync(
        [FromRoute] OnlineStoreDisassociateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new OnlineStoreDisassociateCommand(request.Ids), cancellationToken);
    }
}