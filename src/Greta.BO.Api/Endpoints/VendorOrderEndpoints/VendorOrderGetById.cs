using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

[Route("api/VendorOrder")]
public class VendorOrderGetById : EndpointBaseAsync.WithRequest<VendorOrderGetByIdRequest>.WithActionResult<VendorOrderGetByIdResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Vendor Order by id",
        Description = "Get Vendor Order by id",
        OperationId = "VendorOrder.GetById",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderGetByIdResponse), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderGetByIdResponse>> HandleAsync(
        [FromMultiSource] VendorOrderGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new VendorOrderGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}