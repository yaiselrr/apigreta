using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;

[Route("api/ScaleCategory")]
public class ScaleCategoryFilter: EndpointBaseAsync.WithRequest<ScaleCategoryFilterRequest>.WithActionResult<ScaleCategoryFilterResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the scale category entity",
        Description = "Gets a paginated list of the scale category entity",
        OperationId = "ScaleCategory.Filter",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryFilterResponse), 200)]
    public override async Task<ActionResult<ScaleCategoryFilterResponse>> HandleAsync(
        [FromMultiSource]ScaleCategoryFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleCategoryFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}