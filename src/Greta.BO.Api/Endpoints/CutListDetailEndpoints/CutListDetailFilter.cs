using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.CutListDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;

[Route("api/CutListDetail")]
public class CutListDetailFilter: EndpointBaseAsync.WithRequest<CutListDetailFilterRequest>.WithActionResult<CutListDetailFilterResponse>
{
    private readonly IMediator _mediator;

    public CutListDetailFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("Filter")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the cut list detail entity",
        Description = "Gets a paginated list of the cut list detail entity",
        OperationId = "CutListDetail.Filter",
        Tags = new[] { "CutListDetail" })
    ]
    [ProducesResponseType(typeof(CutListDetailFilterResponse), 200)]
    public override async Task<ActionResult<CutListDetailFilterResponse>> HandleAsync(
        [FromMultiSource]CutListDetailFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListDetailFilterQuery( request.Filter), cancellationToken);
    }
}