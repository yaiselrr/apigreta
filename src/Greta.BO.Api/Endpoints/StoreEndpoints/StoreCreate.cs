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
public class StoreCreate: EndpointBaseAsync.WithRequest<StoreCreateRequest>.WithResult<StoreCreateResponse>
{
    private readonly IMediator _mediator;

    public StoreCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Store",
        Description = "Create a new Store",
        OperationId = "Store.Create",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreGetByIdResponse), 200)]
    public override async Task<StoreCreateResponse> HandleAsync(
        [FromMultiSource] StoreCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new StoreCreateCommand(request.EntityDto), cancellationToken);
    }
}