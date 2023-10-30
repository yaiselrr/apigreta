using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;

[Route("api/MixAndMatch")]
public class MixAndMatchGetById : EndpointBaseAsync.WithRequest<MixAndMatchGetByIdRequest>.WithActionResult<MixAndMatchGetByIdResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get MixAndMatch by id",
        Description = "Get MixAndMatch by id",
        OperationId = "MixAndMatch.GetById",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchGetByIdResponse), 200)]
    public override async Task<ActionResult<MixAndMatchGetByIdResponse>> HandleAsync(
        [FromMultiSource] MixAndMatchGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new MixAndMatchGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}