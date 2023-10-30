using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;
[Route("api/Category")]
public class CategoryChangeState : EndpointBaseAsync.WithRequest<CategoryChangeStateRequest>.WithActionResult<CategoryChangeStateResponse>
{
    private readonly IMediator _mediator;

    public CategoryChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of the category entity",
        Description = "Change State of the category entity",
        OperationId = "Category.ChangeState",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryChangeStateResponse), 200)]
    public override async Task<ActionResult<CategoryChangeStateResponse>> HandleAsync(
        [FromRoute] CategoryChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new CategoryChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}