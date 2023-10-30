using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.PriceBatchDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;
[Route("api/PriceBatchDetail")]
public class PriceBatchDetailGet: EndpointBaseAsync.WithoutRequest.WithActionResult<PriceBatchDetailGetAllResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all price batch detail entities",
        Description = "Get all price batch detail entities",
        OperationId = "PriceBatchDetail.Get",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailGetAllResponse), 200)]
    public override async Task<ActionResult<PriceBatchDetailGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new PriceBatchDetailGetAllQuery(), cancellationToken));
    }
}