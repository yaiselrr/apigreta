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
public class CustomerUpdate : EndpointBaseAsync.WithRequest<CustomerUpdateRequest>.WithResult<CustomerUpdateResponse>
{
    private readonly IMediator _mediator;

    public CustomerUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a customer entity by Id",
        Description = "Update a customer entity by Id",
        OperationId = "Customer.Update",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerGetByIdResponse), 200)]
    public override async Task<CustomerUpdateResponse> HandleAsync(
        [FromMultiSource] CustomerUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CustomerUpdateCommand(request.Id,  request.EntityDto), cancellationToken);
    }
}