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
public class CustomerFilter: EndpointBaseAsync.WithRequest<CustomerFilterRequest>.WithActionResult<CustomerFilterResponse>
{
    private readonly IMediator _mediator;

    public CustomerFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the customer entity",
        Description = "Gets a paginated list of the customer entity",
        OperationId = "Customer.Filter",
        Tags = new[] { "Customer" })
    ]
    [ProducesResponseType(typeof(CustomerFilterResponse), 200)]
    public override async Task<ActionResult<CustomerFilterResponse>> HandleAsync(
        [FromMultiSource]CustomerFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CustomerFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}