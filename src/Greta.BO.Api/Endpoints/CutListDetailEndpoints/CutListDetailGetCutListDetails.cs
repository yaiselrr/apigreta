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
public class CutListDetailGetCutListDetails : EndpointBaseAsync.WithRequest<CutListDetailGetCutListDetailsRequest>.
    WithActionResult<CutListDetailGetCutListDetailsResponse>
{
    private readonly IMediator _mediator;

    public CutListDetailGetCutListDetails(IMediator mediator)
    {
        _mediator = mediator;   
    }

    [HttpGet("GetCutListDetails/{cutListId:int}")]
    [SwaggerOperation(
        Summary = "Get cut list details by cutList",
        Description = "Get cut list details by cutList",
        OperationId = "CutListDetail.GetCutListDetails",
        Tags = new[] { "CutListDetail" })
    ]
    [ProducesResponseType(typeof(CutListDetailGetCutListDetailsResponse), 200)]
    public override async Task<ActionResult<CutListDetailGetCutListDetailsResponse>> HandleAsync(
        [FromMultiSource] CutListDetailGetCutListDetailsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListDetailGetCutListDetailsQuery(request.CutListId), cancellationToken);
    }
}