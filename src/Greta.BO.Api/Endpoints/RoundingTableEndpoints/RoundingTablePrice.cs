using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoundingTableEndpoints;
[Route("api/StoreProduct")]
public class RoundingTablePrice: EndpointBaseAsync.WithRequest<RoundingTablePriceRequest>.WithActionResult<RoundPriceResponse>
{
    private readonly IMediator _mediator;

    public RoundingTablePrice(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("RoundPrice/{price:decimal}")]
    [SwaggerOperation(
        Summary = "Get rounded value",
        Description = "Get rounded value",
        OperationId = "RoundingTable.Get",
        Tags = new[] { "RoundingTable" })
    ]
    [ProducesResponseType(typeof(RoundPriceResponse), 200)]
    public override async Task<ActionResult<RoundPriceResponse>> HandleAsync(
        [FromMultiSource] RoundingTablePriceRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var data = await _mediator.Send(new RoundPriceQuery(request.Price), cancellationToken);
        return data != null ? data : NotFound();
    }
}