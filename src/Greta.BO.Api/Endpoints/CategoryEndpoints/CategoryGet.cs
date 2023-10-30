using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;
[Route("api/Category")]
public class CategoryGet: EndpointBaseAsync.WithoutRequest.WithActionResult<CategoryGetAllResponse>
{
    private readonly IMediator _mediator;

    public CategoryGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all category entities",
        Description = "Get all category entities",
        OperationId = "Category.Get",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryGetAllResponse), 200)]
    public override async Task<ActionResult<CategoryGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new CategoryGetAllQuery(), cancellationToken));
    }
}