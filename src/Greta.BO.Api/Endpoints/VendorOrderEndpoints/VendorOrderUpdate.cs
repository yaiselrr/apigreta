using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

[Route("api/VendorOrder")]
public class VendorOrderUpdate : EndpointBaseAsync.WithRequest<VendorOrderUpdateRequest>.WithActionResult<
    VendorOrderUpdateResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Vendor Order by Id",
        Description = "Update a Vendor Order by Id",
        OperationId = "VendorOrder.Update",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderGetByIdResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderUpdateResponse>> HandleAsync(
        [FromMultiSource] VendorOrderUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new VendorOrderUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}