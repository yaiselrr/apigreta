using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Tax;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;
[Route("api/Tax")]
public class TaxGet: EndpointBaseAsync.WithoutRequest.WithActionResult<TaxGetAllResponse>
{
    private readonly IMediator _mediator;

    public TaxGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Tax Entities",
        Description = "Get all Tax Entities",
        OperationId = "Tax.Get",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxGetAllResponse), 200)]
    public override async Task<ActionResult<TaxGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new TaxGetAllQuery(), cancellationToken));
    }
}