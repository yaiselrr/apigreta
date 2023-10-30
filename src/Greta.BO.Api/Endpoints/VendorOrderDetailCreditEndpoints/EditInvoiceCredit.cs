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
public class EditInvoiceCredit : EndpointBaseAsync.WithRequest<EditInvoiceCreditRequest>.WithActionResult<
    VendorOrderInvoiceCreditEditResponse>
{
    private readonly IMediator _mediator;

    public EditInvoiceCredit(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("EditInvoiceCredit")]
    [SwaggerOperation(
        Summary = "Edit an existing invoice credit of a purchase order",
        Description = "Edit an existing invoice credit of a purchase order",
        OperationId = "VendorOrderDetailCredit.EditInvoiceCredit",
        Tags = new[] { "VendorOrderDetailCredit" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderInvoiceCreditEditResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderInvoiceCreditEditResponse>> HandleAsync(
        [FromMultiSource] EditInvoiceCreditRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new VendorOrderInvoiceCreditEditCommand(request.EntityDto),
                cancellationToken);
    }
}