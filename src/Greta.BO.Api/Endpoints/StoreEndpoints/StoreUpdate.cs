using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Store;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreUpdate : EndpointBaseAsync.WithRequest<StoreUpdateRequest>.WithResult<StoreUpdateResponse>
{
    private readonly IMediator _mediator;

    public StoreUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a store entity by ID",
        Description = "Update a store entity by ID",
        OperationId = "Store.Update",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetByIdResponse), 200)]
    public override async Task<StoreUpdateResponse> HandleAsync(
        [FromMultiSource] StoreUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new StoreUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}