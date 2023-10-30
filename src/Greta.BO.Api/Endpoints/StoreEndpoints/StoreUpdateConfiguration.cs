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
public class StoreUpdateConfiguration : EndpointBaseAsync.WithRequest<StoreUpdateConfigurationRequest>.WithResult<StoreUpdateConfigurationResponse>
{
    private readonly IMediator _mediator;

    public StoreUpdateConfiguration(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("UpdateStoreConfiguration/{entityId}")]
    [SwaggerOperation(
        Summary = "Update of the store entity configuration",
        Description = "Update of the store entity configuration",
        OperationId = "Store.UpdateStoreConfiguration",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetByIdResponse), 200)]
    public override async Task<StoreUpdateConfigurationResponse> HandleAsync(
        [FromMultiSource] StoreUpdateConfigurationRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new StoreUpdateConfigurationCommand(request.Id, request.EntityDto), cancellationToken);
    }
}