using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ShelfTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;

[Route("api/ShelfTag")]
public class ShelfTagAddByCategory : EndpointBaseAsync.WithRequest<ShelfTagAddByCategoryRequest>.WithResult<
    ShelfTagAddByCategoryResponse>
{
    private readonly IMediator _mediator;

    public ShelfTagAddByCategory(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ShelfTagAddByCategory/{categoryId}")]
    [SwaggerOperation(
        Summary = "Add all product to one Category to the ShelfTag print list",
        Description = "Add all product to one Category to the ShelfTag print list",
        OperationId = "ShelfTag.ShelfTagAddByCategory",
        Tags = new[] { "ShelfTag" })
    ]
    [ProducesResponseType(typeof(ShelfTagAddByCategoryResponse), 200)]
    public override async Task<ShelfTagAddByCategoryResponse> HandleAsync(
        [FromMultiSource] ShelfTagAddByCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ShelfTagAddByCategoryCommand(request.CategoryId), cancellationToken);
    }
}