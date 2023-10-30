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
public class VendorOrderDeleteRange : EndpointBaseAsync.WithRequest<VendorOrderDeleteRangeRequest>.WithResult<
    VendorOrderDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of VendorOrder",
        Description = "Delete list of VendorOrder",
        OperationId = "VendorOrder.DeleteRange",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderDeleteRangeResponse), 200)]
    public override async Task<VendorOrderDeleteRangeResponse> HandleAsync(
        [FromMultiSource] VendorOrderDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new VendorOrderDeleteRangeCommand(request.Ids), cancellationToken);
    }
}