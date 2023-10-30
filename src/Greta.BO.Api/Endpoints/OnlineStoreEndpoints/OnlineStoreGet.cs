using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;
[Route("api/OnlineStore")]
public class OnlineStoreGet: EndpointBaseAsync.WithoutRequest.WithActionResult<OnlineStoreGetAllResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all OnlineStore entities",
        Description = "Get all OnlineStore entities",
        OperationId = "OnlineStore.Get",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreGetAllResponse), 200)]
    public override async Task<ActionResult<OnlineStoreGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new OnlineStoreGetAllQuery(), cancellationToken));
    }
}