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
public class MixAndMatchFilter: EndpointBaseAsync.WithRequest<MixAndMatchFilterRequest>.WithActionResult<MixAndMatchFilterResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of MixAndMatch entity",
        Description = "Gets a paginated list of MixAndMatch entity",
        OperationId = "MixAndMatch.Filter",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchFilterResponse), 200)]
    public override async Task<ActionResult<MixAndMatchFilterResponse>> HandleAsync(
        [FromMultiSource]MixAndMatchFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new MixAndMatchFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken));
    }
}