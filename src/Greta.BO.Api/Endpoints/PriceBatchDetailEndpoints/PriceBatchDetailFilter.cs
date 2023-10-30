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
public class PriceBatchDetailFilter: EndpointBaseAsync.WithRequest<PriceBatchDetailFilterRequest>.WithActionResult<PriceBatchDetailFilterResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the price batch detail entity",
        Description = "Gets a paginated list of the price batch detail entity",
        OperationId = "PriceBatchDetail.Filter",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailFilterResponse), 200)]
    public override async Task<ActionResult<PriceBatchDetailFilterResponse>> HandleAsync(
        [FromMultiSource]PriceBatchDetailFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new PriceBatchDetailFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}