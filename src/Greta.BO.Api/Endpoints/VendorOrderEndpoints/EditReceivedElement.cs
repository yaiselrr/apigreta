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
public class EditReceivedElement : EndpointBaseAsync.WithRequest<EditReceivedElementRequest>.WithActionResult<VendorOrderUpdateReceiveResponse>
{
    private readonly IMediator _mediator;

    public EditReceivedElement(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("EditReceivedElement/{entityId:long}")]
    [SwaggerOperation(
        Summary = "Edit received purchase order",
        Description = "Edit received purchase order",
        OperationId = "VendorOrder.EditReceivedElement",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderUpdateReceiveResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderUpdateReceiveResponse>> HandleAsync(
        [FromMultiSource] EditReceivedElementRequest request,
        CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new VendorOrderUpdateReceiveCommand(request.Id, request.EntityDto), cancellationToken);
    }
}