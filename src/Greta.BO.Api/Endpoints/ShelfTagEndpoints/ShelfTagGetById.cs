using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ShelfTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;

[Route("api/ShelfTag")]
public class ShelfTagGetById : EndpointBaseAsync.WithRequest<ShelfTagGetByIdRequest>.WithActionResult<ShelfTagGetByIdResponse>
{
    private readonly IMediator _mediator;

    public ShelfTagGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get ShelfTag entity by id",
        Description = "Get ShelfTag entity by id",
        OperationId = "ShelfTag.GetById",
        Tags = new[] { "ShelfTag" })
    ]
    [ProducesResponseType(typeof(ShelfTagGetByIdResponse), 200)]
    public override async Task<ActionResult<ShelfTagGetByIdResponse>> HandleAsync(
        [FromMultiSource] ShelfTagGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ShelfTagGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}