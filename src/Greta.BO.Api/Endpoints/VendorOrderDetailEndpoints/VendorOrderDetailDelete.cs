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
public class VendorOrderDetailDelete : EndpointBaseAsync.WithRequest<VendorOrderDetailDeleteRequest>.WithActionResult<
    VendorOrderDetailDeleteResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailDelete(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Vendor Order detail by Id",
        Description = "Delete a Vendor Order detail by Id",
        OperationId = "VendorOrderDetail.Delete",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderDetailDeleteResponse), 200)]
    public override async Task<ActionResult<VendorOrderDetailDeleteResponse>> HandleAsync(
        [FromMultiSource] VendorOrderDetailDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new VendorOrderDetailDeleteCommand(request.Id), cancellationToken);
    }
}