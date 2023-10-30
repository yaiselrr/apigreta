using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetailCredit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailCreditEndpoints;

[Route("api/VendorOrderDetailCredit")]
public class SendCreditMail : EndpointBaseAsync.WithRequest<SendCreditMailRequest>.WithActionResult<
    VendorOrderInvoiceCreditSendEmailResponse>
{
    private readonly IMediator _mediator;

    public SendCreditMail(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("SendCreditMail/{entityId}")]
    [SwaggerOperation(
        Summary = "Send mail to vendor with credits",
        Description = "Send mail to vendor with credits",
        OperationId = "VendorOrderDetailCredit.SendCreditMail",
        Tags = new[] { "VendorOrderDetailCredit" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderInvoiceCreditSendEmailResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderInvoiceCreditSendEmailResponse>> HandleAsync(
        [FromMultiSource] SendCreditMailRequest request,
        CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new VendorOrderInvoiceCreditSendEmailCommand(request.Id),
                cancellationToken);
    }
}