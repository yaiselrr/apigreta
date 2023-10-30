using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Discount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;

[Route("api/Discount")]
public class DiscountGetById : EndpointBaseAsync.WithRequest<DiscountGetByIdRequest>.WithActionResult<DiscountGetByIdResponse>
{
    private readonly IMediator _mediator;

    public DiscountGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get discount entity by id",
        Description = "Get discount entity by id",
        OperationId = "Discount.GetById",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountGetByIdResponse), 200)]
    public override async Task<ActionResult<DiscountGetByIdResponse>> HandleAsync(
        [FromMultiSource] DiscountGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new DiscountGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}