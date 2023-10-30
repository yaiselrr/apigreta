using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;
[Route("api/OnlineStore")]
public class OnlineStoreDelete : EndpointBaseAsync.WithRequest<OnlineStoreDeleteRequest>.WithActionResult<OnlineStoreDeleteResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a OnlineStore entity by Id",
        Description = "Delete a OnlineStore entity by Id",
        OperationId = "OnlineStore.Delete",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreDeleteResponse), 200)]
    public override async Task<ActionResult<OnlineStoreDeleteResponse>> HandleAsync(
        [FromMultiSource] OnlineStoreDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new OnlineStoreDeleteCommand(request.Id), cancellationToken);
    }
}