using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

[Route("api/VendorOrderDetail")]
public class VendorOrderDetailDeleteRange : EndpointBaseAsync.WithRequest<VendorOrderDetailDeleteRangeRequest>.
    WithResult<VendorOrderDetailDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Vendor Order detail",
        Description = "Delete list of Vendor Order detail",
        OperationId = "VendorOrderDetail.DeleteRange",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderDetailDeleteRangeResponse), 200)]
    public override async Task<VendorOrderDetailDeleteRangeResponse> HandleAsync(
        [FromMultiSource] VendorOrderDetailDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new VendorOrderDetailDeleteRangeCommand(request.Ids), cancellationToken);
    }
}