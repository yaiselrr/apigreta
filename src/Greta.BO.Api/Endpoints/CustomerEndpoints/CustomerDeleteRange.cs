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
public class CustomerDeleteRange: EndpointBaseAsync.WithRequest<CustomerDeleteRangeRequest>.WithResult<CustomerDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public CustomerDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of the customer entities",
        Description = "Delete list of the customer entities",
        OperationId = "Customer.DeleteRange",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerDeleteRangeResponse), 200)]
    public override async Task<CustomerDeleteRangeResponse> HandleAsync(
        [FromMultiSource] CustomerDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CustomerDeleteRangeCommand(request.Ids), cancellationToken);
    }
}