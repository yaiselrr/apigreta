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
public class OnlineStoreDeleteRange: EndpointBaseAsync.WithRequest<OnlineStoreDeleteRangeRequest>.WithResult<OnlineStoreDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of OnlineStore entities",
        Description = "Delete list of OnlineStore entities",
        OperationId = "OnlineStore.DeleteRange",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreDeleteRangeResponse), 200)]
    public override async Task<OnlineStoreDeleteRangeResponse> HandleAsync(
        [FromMultiSource] OnlineStoreDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new OnlineStoreDeleteRangeCommand(request.Ids), cancellationToken);
    }
}