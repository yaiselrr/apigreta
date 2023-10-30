using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;
[Route("api/Category")]
public class CategoryDelete : EndpointBaseAsync.WithRequest<CategoryDeleteRequest>.WithActionResult<CategoryDeleteResponse>
{
    private readonly IMediator _mediator;

    public CategoryDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a category entity by Id",
        Description = "Delete a category entity by Id",
        OperationId = "Category.Delete",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryDeleteResponse), 200)]
    public override async Task<ActionResult<CategoryDeleteResponse>> HandleAsync(
        [FromMultiSource] CategoryDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new CategoryDeleteCommand(request.Id), cancellationToken);
    }
}