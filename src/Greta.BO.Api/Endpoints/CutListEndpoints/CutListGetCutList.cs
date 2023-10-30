using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.CutList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

[Route("api/CutList")]
public class CutListGetCutList : EndpointBaseAsync.WithRequest<CutListGetCutListRequest>.
    WithActionResult<CutListGetCutListResponse>
{
    private readonly IMediator _mediator;

    public CutListGetCutList(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetCutList/{animalId:int}/{customerId:int}/{includeDetails}")]
    [SwaggerOperation(
        Summary = "Get cut list by animal and customer",
        Description = "Get cut list by animal and customer",
        OperationId = "CutList.GetCutList",
        Tags = new[] { "CutList" })
    ]
    [ProducesResponseType(typeof(CutListGetCutListResponse), 200)]
    public override async Task<ActionResult<CutListGetCutListResponse>> HandleAsync(
        [FromMultiSource] CutListGetCutListRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListGetCutListQuery(request.AnimalId, request.CustomerId, request.IncludeDetails), cancellationToken);
    }
}