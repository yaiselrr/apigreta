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
public class CategoryCreate: EndpointBaseAsync.WithRequest<CategoryCreateRequest>.WithResult<CategoryCreateResponse>
{
    private readonly IMediator _mediator;

    public CategoryCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new category entity",
        Description = "Create a new category entity",
        OperationId = "Category.Create",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryGetByIdResponse), 200)]
    public override async Task<CategoryCreateResponse> HandleAsync(
        [FromMultiSource] CategoryCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CategoryCreateCommand(request.EntityDto), cancellationToken);
    }
}