using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;
[Route("api/Customer")]
public class CustomerChangeState : EndpointBaseAsync.WithRequest<CustomerChangeStateRequest>.WithActionResult<CustomerChangeStateResponse>
{
    private readonly IMediator _mediator;

    public CustomerChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the customer entity",
        Description = "Change the state of the customer entity",
        OperationId = "Customer.ChangeState",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerChangeStateResponse), 200)]
    public override async Task<ActionResult<CustomerChangeStateResponse>> HandleAsync(
        [FromRoute] CustomerChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new CustomerChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}