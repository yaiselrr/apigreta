using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

[Route("api/Family")]
public class AddProductsToFamily: EndpointBaseAsync.WithRequest<AddProductsToFamilyRequest>.WithActionResult<AddProductsToFamilyResponse>
{
    private readonly IMediator _mediator;

    public AddProductsToFamily(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("AddProductsToFamily/{entityId:long}")]
    [SwaggerOperation(
        Summary = "Add a product to a family",
        Description = "Add a product to a family",
        OperationId = "Family.AddProduct",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(AddProductsToFamilyResponse), 200)]
    public override async Task<ActionResult<AddProductsToFamilyResponse>> HandleAsync(
        [FromMultiSource]AddProductsToFamilyRequest request, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new AddProductsToFamilyCommand(request.EntityId, request.Upcs), cancellationToken));
    }
}