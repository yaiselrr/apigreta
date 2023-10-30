using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Discount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;
[Route("api/Discount")]
public class DiscountGet : EndpointBaseAsync.WithoutRequest.WithActionResult<DiscountGetAllResponse>
{
    private readonly IMediator _mediator;

    public DiscountGet(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Discount Entities",
        Description = "Get all Discount Entities",
        OperationId = "Discount.Get",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountGetAllResponse), 200)]
    public override async Task<ActionResult<DiscountGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new DiscountGetAllQuery(), cancellationToken));
    }
}