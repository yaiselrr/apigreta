using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Customer;
using Greta.BO.BusinessLogic.Handlers.Queries.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;
[Route("api/Customer")]
public class CustomerCreate: EndpointBaseAsync.WithRequest<CustomerCreateRequest>.WithResult<CustomerCreateResponse>
{
    private readonly IMediator _mediator;

    public CustomerCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new customer entity",
        Description = "Create a new customer entity",
        OperationId = "Customer.Create",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerGetByIdResponse), 200)]
    public override async Task<CustomerCreateResponse> HandleAsync(
        [FromMultiSource] CustomerCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CustomerCreateCommand(request.EntityDto), cancellationToken);
    }
}