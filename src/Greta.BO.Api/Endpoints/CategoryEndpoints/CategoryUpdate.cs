using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Category;
using Greta.BO.BusinessLogic.Handlers.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;
[Route("api/Category")]
public class CategoryUpdate : EndpointBaseAsync.WithRequest<CategoryUpdateRequest>.WithResult<CategoryUpdateResponse>
{
    private readonly IMediator _mediator;

    public CategoryUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}/{changeAllProducts}")]
    [SwaggerOperation(
        Summary = "Update a category entity by Id",
        Description = "Update a category entity by Id",
        OperationId = "Category.Update",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryGetByIdResponse), 200)]
    public override async Task<CategoryUpdateResponse> HandleAsync(
        [FromMultiSource] CategoryUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CategoryUpdateCommand(request.Id, request.changeAllProducts, request.EntityDto), cancellationToken);
    }
}