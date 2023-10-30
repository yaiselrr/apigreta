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
public class FilterFamilyProductsNotIncluded : EndpointBaseAsync.WithRequest<FilterFamilyProductsNotIncludedRequest>.
    WithActionResult<ProductFilterNotIncludedInFamilyResponse>
{
    private readonly IMediator _mediator;

    public FilterFamilyProductsNotIncluded(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ProductsNotIncludedFamily/{familyId}/{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Get filter and paginate Products haven't relation with family",
        Description = "Get filter and paginate Products haven't relation with family",
        OperationId = "Family.FilterFamilyProductsNotIncluded",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(ProductFilterNotIncludedInFamilyResponse), 200)]
    public override async Task<ActionResult<ProductFilterNotIncludedInFamilyResponse>> HandleAsync(
        [FromMultiSource] FilterFamilyProductsNotIncludedRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(
            new ProductFilterNotIncludedInFamilyQuery(request.FamilyId, request.CurrentPage, request.PageSize,
                request.Filter), cancellationToken);
    }
}