using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;
[Route("api/Family")]
public class FilterFamilyProducts: EndpointBaseAsync.WithRequest<FamilyFilterRequest>
    .WithActionResult<FamilyProductsResponse>
{
    private readonly IMediator _mediator;

    public FilterFamilyProducts(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("FamilyProducts/{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Get filter and paginate Products of family",
        Description = "Get filter and paginate Products of family",
        OperationId = "Family.FilterFamilyProducts",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyFilterResponse), 200)]
    public override async Task<ActionResult<FamilyProductsResponse>> HandleAsync(
        [FromMultiSource]FamilyFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FamilyProductsFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}