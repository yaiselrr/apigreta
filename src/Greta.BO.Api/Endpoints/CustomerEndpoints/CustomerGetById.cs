using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;

[Route("api/Customer")]
public class CustomerGetById : EndpointBaseAsync.WithRequest<CustomerGetByIdRequest>.WithActionResult<CustomerGetByIdResponse>
{
    private readonly IMediator _mediator;

    public CustomerGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get category entity by id",
        Description = "Get category entity by id",
        OperationId = "Customer.GetById",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerGetByIdResponse), 200)]
    public override async Task<ActionResult<CustomerGetByIdResponse>> HandleAsync(
        [FromMultiSource] CustomerGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new CustomerGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}