using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.PriceBatchDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;

[Route("api/PriceBatchDetail")]
public class PriceBatchDetailGetById : EndpointBaseAsync.WithRequest<PriceBatchDetailGetByIdRequest>.WithActionResult<PriceBatchDetailGetByIdResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get price batch detail entity by id",
        Description = "Get price batch detail entity by id",
        OperationId = "PriceBatchDetail.GetById",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailGetByIdResponse), 200)]
    public override async Task<ActionResult<PriceBatchDetailGetByIdResponse>> HandleAsync(
        [FromMultiSource] PriceBatchDetailGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new PriceBatchDetailGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}