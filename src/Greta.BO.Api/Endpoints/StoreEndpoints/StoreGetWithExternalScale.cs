using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreGetWithExternalScale: EndpointBaseAsync.WithoutRequest.WithActionResult<StoreGetWithExternalScaleResponse>
{
    private readonly IMediator _mediator;

    public StoreGetWithExternalScale(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetWithExternalScale")]
    [SwaggerOperation(
        Summary = "Get  with external scale store Entities",
        Description = "Get  with external scale store Entities",
        OperationId = "Store.GetWithExternalScale",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetWithExternalScaleResponse), 200)]
    public override async Task<ActionResult<StoreGetWithExternalScaleResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new StoreGetWithExternalScaleQuery(), cancellationToken));
    }
}