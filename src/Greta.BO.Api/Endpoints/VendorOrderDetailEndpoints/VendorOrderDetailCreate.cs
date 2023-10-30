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
public class VendorOrderDetailCreate : EndpointBaseAsync.WithRequest<VendorOrderDetailCreateRequest>.WithResult<
    VendorOrderDetailCreateResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailCreate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Vendor Order detail",
        Description = "Create a new Vendor Order detail",
        OperationId = "VendorOrderDetail.Create",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderDetailCreateResponse), 200)]
    public override async Task<VendorOrderDetailCreateResponse> HandleAsync(
        [FromMultiSource] VendorOrderDetailCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new VendorOrderDetailCreateCommand(request.EntityDto), cancellationToken);
    }
}