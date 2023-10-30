using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;

[Route("api/Grind")]
public class GrindGetById : EndpointBaseAsync.WithRequest<GrindGetByIdRequest>.WithActionResult<GrindGetByIdResponse>
{
    private readonly IMediator _mediator;

    public GrindGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Grind entity by id",
        Description = "Get Grind entity by id",
        OperationId = "Grind.GetById",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindGetByIdResponse), 200)]
    public override async Task<ActionResult<GrindGetByIdResponse>> HandleAsync(
        [FromMultiSource] GrindGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new GrindGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}