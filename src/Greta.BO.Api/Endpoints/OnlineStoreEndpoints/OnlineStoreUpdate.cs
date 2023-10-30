using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;
using Greta.BO.BusinessLogic.Handlers.Queries.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;
[Route("api/OnlineStore")]
public class OnlineStoreUpdate : EndpointBaseAsync.WithRequest<OnlineStoreUpdateRequest>.WithResult<OnlineStoreUpdateResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a OnlineStore entity by Id",
        Description = "Update a OnlineStore entity by Id",
        OperationId = "OnlineStore.Update",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreGetByIdResponse), 200)]
    public override async Task<OnlineStoreUpdateResponse> HandleAsync(
        [FromMultiSource] OnlineStoreUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new OnlineStoreUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}