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
public class CategoryGetById : EndpointBaseAsync.WithRequest<CategoryGetByIdRequest>.WithActionResult<CategoryGetByIdResponse>
{
    private readonly IMediator _mediator;

    public CategoryGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get category entity by id",
        Description = "Get category entity by id",
        OperationId = "Category.GetById",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryGetByIdResponse), 200)]
    public override async Task<ActionResult<CategoryGetByIdResponse>> HandleAsync(
        [FromMultiSource] CategoryGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new CategoryGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}