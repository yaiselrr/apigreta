using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;
[Route("api/ScaleCategory")]
public class ScaleCategoryCsv: EndpointBaseAsync.WithoutRequest.WithActionResult<ScaleCategoryGetAllResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryCsv(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("Csv")]
    [SwaggerOperation(
        Summary = "Get all scale category entities to csv",
        Description = "Get all scale category entities to csv",
        OperationId = "ScaleCategory.Csv",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryGetAllResponse), 200)]
    public override async Task<ActionResult<ScaleCategoryGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new ScaleCategoryGetAllQuery(), cancellationToken));
    }
}