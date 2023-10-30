using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreGetStoresWithExternalScale: EndpointBaseAsync.WithoutRequest.WithActionResult<StoreGetWithExternalScaleResponse>
{
    private readonly IMediator _mediator;

    public StoreGetStoresWithExternalScale(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetStoresWithExternalScale")]
    [SwaggerOperation(
        Summary = "Get store Entities with external scale",
        Description = "Get store Entities with external scale",
        OperationId = "Store.GetStoresWithExternalScale",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetWithExternalScaleResponse), 200)]
    public override async Task<ActionResult<StoreGetWithExternalScaleResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new StoreGetWithExternalScaleQuery(), cancellationToken));
    }
}