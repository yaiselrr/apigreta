using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;
[Route("api/Customer")]
public class CustomerGet: EndpointBaseAsync.WithoutRequest.WithActionResult<CustomerGetAllResponse>
{
    private readonly IMediator _mediator;

    public CustomerGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all customer entities",
        Description = "Get all customer entities",
        OperationId = "Customer.Get",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerGetAllResponse), 200)]
    public override async Task<ActionResult<CustomerGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new CustomerGetAllQuery(), cancellationToken));
    }
}