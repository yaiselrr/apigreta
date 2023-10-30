using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

[Route("api/VendorOrderDetail")]
public class VendorOrderDetailGetById : EndpointBaseAsync.WithRequest<VendorOrderDetailGetByIdRequest>.WithActionResult<
    VendorOrderDetailGetByIdResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Vendor Order detail by id",
        Description = "Get Vendor Order detail by id",
        OperationId = "VendorOrderDetail.GetById",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderDetailGetByIdResponse), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderDetailGetByIdResponse>> HandleAsync(
        [FromMultiSource] VendorOrderDetailGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new VendorOrderDetailGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}