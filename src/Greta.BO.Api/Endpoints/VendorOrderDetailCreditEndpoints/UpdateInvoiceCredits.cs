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
public class UpdateInvoiceCredits : EndpointBaseAsync.WithRequest<UpdateInvoiceCreditsRequest>.WithActionResult<
    VendorOrderInvoiceCreditResponse>
{
    private readonly IMediator _mediator;

    public UpdateInvoiceCredits(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("UpdateInvoiceCredits/{entityId}")]
    [SwaggerOperation(
        Summary = "Update an invoice credit of a purchase order",
        Description = "Update an invoice credit of a purchase order",
        OperationId = "VendorOrderDetailCredit.UpdateInvoiceCredits",
        Tags = new[] { "VendorOrderDetailCredit" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderInvoiceCreditResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderInvoiceCreditResponse>> HandleAsync(
        [FromMultiSource] UpdateInvoiceCreditsRequest request,
        CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new VendorOrderInvoiceCreditCommand(request.Id, request.EntityDto),
                cancellationToken);
    }
}