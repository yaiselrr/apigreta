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
public class VendorOrderDetailUpdate : EndpointBaseAsync.WithRequest<VendorOrderDetailUpdateRequest>.WithActionResult<
    VendorOrderDetailUpdateResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Vendor Order detail by Id",
        Description = "Update a Vendor Order detail by Id",
        OperationId = "VendorOrderDetail.Update",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderDetailUpdateResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderDetailUpdateResponse>> HandleAsync(
        [FromMultiSource] VendorOrderDetailUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new VendorOrderDetailUpdateCommand(request.Id, request.EntityDto),
                cancellationToken);
    }
}