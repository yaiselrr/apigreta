using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.CutList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

[Route("api/CutList")]
public class CutListGetScaleProductByUpcAndPlu : EndpointBaseAsync.WithRequest<CutListGetScaleProductByUpcAndPluRequest>.
    WithActionResult<CutListGetScaleProductByUpcAndPluResponse>
{
    private readonly IMediator _mediator;

    public CutListGetScaleProductByUpcAndPlu(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetScaleProductsByUpcAndPlu/{upc}/{pluNumber:int}/{animalId}")]
    [SwaggerOperation(
        Summary = "Get list of scale products by upc and plu",
        Description = "Get list of scale products by upc and plu",
        OperationId = "CutList.GetScaleProductsByUpcAndPlu",
        Tags = new[] { "CutList" })
    ]
    [ProducesResponseType(typeof(CutListGetScaleProductByUpcAndPluResponse), 200)]
    public override async Task<ActionResult<CutListGetScaleProductByUpcAndPluResponse>> HandleAsync(
        [FromRoute] CutListGetScaleProductByUpcAndPluRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListGetScaleProductByUpcAndPluQuery(request.Upc, request.Plu, request.AnimalId), cancellationToken);
    }
}