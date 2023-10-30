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
public class DiscountFilter: EndpointBaseAsync.WithRequest<DiscountFilterRequest>.WithActionResult<DiscountFilterResponse>
{
    private readonly IMediator _mediator;

    public DiscountFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the Discount entity",
        Description = "Gets a paginated list of the Discount entity",
        OperationId = "Discount.Filter",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountFilterResponse), 200)]
    public override async Task<ActionResult<DiscountFilterResponse>> HandleAsync(
        [FromMultiSource]DiscountFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DiscountFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}