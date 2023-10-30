using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreGetTimezonesId: EndpointBaseAsync.WithoutRequest.WithActionResult<StoreGetTimezonesIdResponse>
{
    private readonly IMediator _mediator;

    public StoreGetTimezonesId(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetTimezonesId")]
    [SwaggerOperation(
        Summary = "Get list of time zones id stores Entities",
        Description = "Get list of time zones id stores Entities",
        OperationId = "Store.GetTimezonesId",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetTimezonesIdResponse), 200)]
    public override async Task<ActionResult<StoreGetTimezonesIdResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new StoreGetTimezonesIdQuery(), cancellationToken));
    }
}