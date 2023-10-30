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
public class OnlineStoreCreate: EndpointBaseAsync.WithRequest<OnlineStoreCreateRequest>.WithResult<OnlineStoreCreateResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new OnlineStore entity",
        Description = "Create a new OnlineStore entity",
        OperationId = "OnlineStore.Create",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreGetByIdResponse), 200)]
    public override async Task<OnlineStoreCreateResponse> HandleAsync(
        [FromMultiSource] OnlineStoreCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new OnlineStoreCreateCommand(request.EntityDto), cancellationToken);
    }
}