using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;
[Route("api/Family")]
public class FamilyGet: EndpointBaseAsync.WithoutRequest.WithActionResult<FamilyGetAllResponse>
{
    private readonly IMediator _mediator;

    public FamilyGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all family Entities",
        Description = "Get all family Entities",
        OperationId = "Family.Get",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyGetAllResponse), 200)]
    public override async Task<ActionResult<FamilyGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new FamilyGetAllQuery(), cancellationToken));
    }
}