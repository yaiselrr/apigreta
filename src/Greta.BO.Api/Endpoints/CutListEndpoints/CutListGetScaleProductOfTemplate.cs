using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.CutList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

[Route("api/CutList")]
public class CutListGetScaleProductOfTemplate : EndpointBaseAsync.WithRequest<CutListGetScaleProductOfTemplateRequest>.
    WithActionResult<CutListGetScaleProductOfTemplateResponse>
{
    private readonly IMediator _mediator;

    public CutListGetScaleProductOfTemplate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetScaleProductsOfTemplate/{cutListTemplateId}/{animalId}")]
    [SwaggerOperation(
        Summary = "Get list of scale products of template",
        Description = "Get list of scale products of template",
        OperationId = "CutList.GetScaleProductsOfTemplate",
        Tags = new[] { "CutList" })
    ]
    [ProducesResponseType(typeof(CutListGetScaleProductOfTemplateResponse), 200)]
    public override async Task<ActionResult<CutListGetScaleProductOfTemplateResponse>> HandleAsync(
        [FromRoute] CutListGetScaleProductOfTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListGetScaleProductOfTemplateQuery(request.CutListTemplateId, request.AnimalId), cancellationToken);
    }
}