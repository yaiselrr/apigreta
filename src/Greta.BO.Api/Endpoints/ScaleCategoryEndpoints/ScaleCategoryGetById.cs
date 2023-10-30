using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;

[Route("api/ScaleCategory")]
public class ScaleCategoryGetById : EndpointBaseAsync.WithRequest<ScaleCategoryGetByIdRequest>.WithActionResult<ScaleCategoryGetByIdResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get scale category entity by id",
        Description = "Get scale category entity by id",
        OperationId = "ScaleCategory.GetById",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryGetByIdResponse), 200)]
    public override async Task<ActionResult<ScaleCategoryGetByIdResponse>> HandleAsync(
        [FromMultiSource] ScaleCategoryGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ScaleCategoryGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}