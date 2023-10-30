using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;
[Route("api/MixAndMatch")]
public class MixAndMatchCreate : EndpointBaseAsync.WithRequest<MixAndMatchCreateRequest>.WithActionResult<MixAndMatchCreateResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchCreate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new MixAndMatch",
        Description = "Create a new MixAndMatch",
        OperationId = "MixAndMatch.Create",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchCreateResponse), 201)]
    public override async Task<ActionResult<MixAndMatchCreateResponse>> HandleAsync(
        [FromMultiSource] MixAndMatchCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new MixAndMatchCreateCommand(request.EntityDto), cancellationToken);
        return result.Data ? Created(string.Empty, result): BadRequest();
    }
}