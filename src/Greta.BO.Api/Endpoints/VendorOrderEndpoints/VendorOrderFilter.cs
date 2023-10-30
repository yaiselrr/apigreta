using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

[Route("api/VendorOrder")]
public class VendorOrderFilter : EndpointBaseAsync.WithRequest<VendorOrderFilterRequest>.WithActionResult<
    VendorOrderFilterResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderFilter(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of vendor order entity",
        Description = "Gets a paginated list of vendor order entity",
        OperationId = "VendorOrder.Filter",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderFilterResponse), 200)]
    public override async Task<ActionResult<VendorOrderFilterResponse>> HandleAsync(
        [FromMultiSource] VendorOrderFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new VendorOrderFilterQuery(request.CurrentPage, request.PageSize, request.Filter),
            cancellationToken);
    }
}