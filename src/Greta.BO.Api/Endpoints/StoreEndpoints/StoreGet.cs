using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreGet: EndpointBaseAsync.WithoutRequest.WithActionResult<StoreGetAllResponse>
{
    private readonly IMediator _mediator;

    public StoreGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all store entities",
        Description = "Get all store entities",
        OperationId = "Store.Get",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetAllResponse), 200)]
    public override async Task<ActionResult<StoreGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new StoreGetAllQuery(), cancellationToken));
    }
}