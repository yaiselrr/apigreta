using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Fee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;

[Route("api/Fee")]
public class FeeGetById : EndpointBaseAsync.WithRequest<FeeGetByIdRequest>.WithActionResult<FeeGetByIdResponse>
{
    private readonly IMediator _mediator;

    public FeeGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Fee by id",
        Description = "Get Fee by id",
        OperationId = "Fee.GetById",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeGetByIdResponse), 200)]
    public override async Task<ActionResult<FeeGetByIdResponse>> HandleAsync(
        [FromMultiSource] FeeGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new FeeGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}