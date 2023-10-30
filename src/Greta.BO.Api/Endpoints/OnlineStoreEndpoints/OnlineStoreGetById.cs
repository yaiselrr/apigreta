using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;

[Route("api/OnlineStore")]
public class OnlineStoreGetById : EndpointBaseAsync.WithRequest<OnlineStoreGetByIdRequest>.WithActionResult<OnlineStoreGetByIdResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get OnlineStore entity by id",
        Description = "Get OnlineStore entity by id",
        OperationId = "OnlineStore.GetById",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreGetByIdResponse), 200)]
    public override async Task<ActionResult<OnlineStoreGetByIdResponse>> HandleAsync(
        [FromMultiSource] OnlineStoreGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new OnlineStoreGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}