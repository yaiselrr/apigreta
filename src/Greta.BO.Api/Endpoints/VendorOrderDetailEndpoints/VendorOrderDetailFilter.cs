using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

[Route("api/VendorOrderDetail")]
public class VendorOrderDetailFilter : EndpointBaseAsync.WithRequest<VendorOrderDetailFilterRequest>.WithActionResult<
    VendorOrderDetailFilterResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailFilter(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Filter")]
    [SwaggerOperation(
        Summary = "Gets a list of vendor order detail entity",
        Description = "Gets a list of vendor order detail entity",
        OperationId = "VendorOrderDetail.Filter",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderDetailFilterResponse), 200)]
    public override async Task<ActionResult<VendorOrderDetailFilterResponse>> HandleAsync(
        [FromMultiSource] VendorOrderDetailFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new VendorOrderDetailFilterQuery(request.Filter),
            cancellationToken);
    }
}