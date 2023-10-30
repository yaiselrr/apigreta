using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;

[Route("api/Store")]
public class StoreGetById : EndpointBaseAsync.WithRequest<StoreGetByIdRequest>.WithActionResult<StoreGetByIdResponse>
{
    private readonly IMediator _mediator;

    public StoreGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get store entity by id",
        Description = "Get store entity by id",
        OperationId = "Store.GetById",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetByIdResponse), 200)]
    public override async Task<ActionResult<StoreGetByIdResponse>> HandleAsync(
        [FromMultiSource] StoreGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new StoreGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}