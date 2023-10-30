using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreDelete : EndpointBaseAsync.WithRequest<StoreDeleteRequest>.WithActionResult<StoreDeleteResponse>
{
    private readonly IMediator _mediator;

    public StoreDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Store by Id",
        Description = "Delete a Store by Id",
        OperationId = "Store.Delete",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreDeleteResponse), 200)]
    public override async Task<ActionResult<StoreDeleteResponse>> HandleAsync(
        [FromMultiSource] StoreDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new StoreDeleteCommand(request.Id), cancellationToken);
    }
}