using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;
[Route("api/Category")]
public class CategoryChangeLiquor : EndpointBaseAsync.WithRequest<CategoryChangeLiquorRequest>.WithActionResult<CategoryChangeLiquorResponse>
{
    private readonly IMediator _mediator;

    public CategoryChangeLiquor(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeLiquor/{entityId}/{isLiquorActive}")]
    [SwaggerOperation(
        Summary = "Change Is Liquor of the category entity",
        Description = "Change Is Liquor of the category entity",
        OperationId = "Category.ChangeLiquor",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryChangeLiquorResponse), 200)]
    public override async Task<ActionResult<CategoryChangeLiquorResponse>> HandleAsync(
        [FromRoute] CategoryChangeLiquorRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new CategoryChangeLiquorCommand(request.Id, request.IsLiquorActive), cancellationToken);
    }
}