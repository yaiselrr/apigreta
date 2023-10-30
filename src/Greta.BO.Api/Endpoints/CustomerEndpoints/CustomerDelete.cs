using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;
[Route("api/Customer")]
public class CustomerDelete : EndpointBaseAsync.WithRequest<CustomerDeleteRequest>.WithActionResult<CustomerDeleteResponse>
{
    private readonly IMediator _mediator;

    public CustomerDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a customer entity by Id",
        Description = "Delete a customer entity by Id",
        OperationId = "Customer.Delete",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerDeleteResponse), 200)]
    public override async Task<ActionResult<CustomerDeleteResponse>> HandleAsync(
        [FromMultiSource] CustomerDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new CustomerDeleteCommand(request.Id), cancellationToken);
    }
}