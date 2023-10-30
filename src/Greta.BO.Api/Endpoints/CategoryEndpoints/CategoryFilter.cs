using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

[Route("api/Category")]
public class CategoryFilter: EndpointBaseAsync.WithRequest<CategoryFilterRequest>.WithActionResult<CategoryFilterResponse>
{
    private readonly IMediator _mediator;

    public CategoryFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the category entity",
        Description = "Gets a paginated list of the category entity",
        OperationId = "Category.Filter",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryFilterResponse), 200)]
    public override async Task<ActionResult<CategoryFilterResponse>> HandleAsync(
        [FromMultiSource]CategoryFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CategoryFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}