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
public class VendorOrderDelete : EndpointBaseAsync.WithRequest<VendorOrderDeleteRequest>.WithActionResult<VendorOrderDeleteResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Vendor Order by Id",
        Description = "Delete a Vendor Order by Id",
        OperationId = "VendorOrder.Delete",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderDeleteResponse), 200)]
    public override async Task<ActionResult<VendorOrderDeleteResponse>> HandleAsync(
        [FromMultiSource] VendorOrderDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new VendorOrderDeleteCommand(request.Id), cancellationToken);
    }
}