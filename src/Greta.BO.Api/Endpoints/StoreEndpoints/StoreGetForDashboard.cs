using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreGetForDashboard: EndpointBaseAsync.WithoutRequest.WithActionResult<StoreGetForDashboardAllResponse>
{
    private readonly IMediator _mediator;

    public StoreGetForDashboard(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetForDashboard")]
    [SwaggerOperation(
        Summary = "Get all store Entities for dashboard",
        Description = "Get all store Entities for dashboard",
        OperationId = "Store.GetForDashboard",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetForDashboardAllResponse), 200)]
    public override async Task<ActionResult<StoreGetForDashboardAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new StoreGetForDashboardAllQuery(), cancellationToken));
    }
}