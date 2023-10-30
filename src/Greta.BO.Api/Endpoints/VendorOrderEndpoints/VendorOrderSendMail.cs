using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

[Route("api/VendorOrder")]
public class VendorOrderSendMail : EndpointBaseAsync.WithRequest<VendorOrderSendMailRequest>.WithActionResult<
    VendorOrderSendEmailResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderSendMail(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("SendMail/{entityId:long}")]
    [SwaggerOperation(
        Summary = "Send mail with purchase order to vendor",
        Description = "Send mail with purchase order to vendor",
        OperationId = "VendorOrder.SendEmail",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderSendEmailResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderSendEmailResponse>> HandleAsync(
        [FromMultiSource] VendorOrderSendMailRequest request,
        CancellationToken cancellationToken = default)
    {
        return (request.Id < 1)? NotFound() : Ok(await _mediator.Send(new VendorOrderSendEmailCommand(request.Id), cancellationToken));
    }
}