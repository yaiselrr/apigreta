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
public class StoreDeleteRange: EndpointBaseAsync.WithRequest<StoreDeleteRangeRequest>.WithResult<StoreDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public StoreDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of store entities",
        Description = "Delete list of store entities",
        OperationId = "Store.DeleteRange",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreDeleteRangeResponse), 200)]
    public override async Task<StoreDeleteRangeResponse> HandleAsync(
        [FromMultiSource] StoreDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new StoreDeleteRangeCommand(request.Ids), cancellationToken);
    }
}