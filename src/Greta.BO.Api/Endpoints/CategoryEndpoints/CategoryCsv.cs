using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;
[Route("api/Category")]
public class CategoryCsv: EndpointBaseAsync.WithoutRequest.WithActionResult<CategoryGetAllResponse>
{
    private readonly IMediator _mediator;

    public CategoryCsv(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("Csv")]
    [SwaggerOperation(
        Summary = "Get all category entities to csv",
        Description = "Get all category entities to csv",
        OperationId = "Category.Csv",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryGetAllResponse), 200)]
    public override async Task<ActionResult<CategoryGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new CategoryGetAllQuery(), cancellationToken));
    }
}