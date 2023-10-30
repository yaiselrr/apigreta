using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;
[Route("api/CutListTemplate")]
public class CutListTemplateGet : EndpointBaseAsync.WithoutRequest.WithActionResult<CutListTemplateGetAllResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateGet(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all CutListTemplate Entities",
        Description = "Get all CutListTemplate Entities",
        OperationId = "CutListTemplate.Get",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateGetAllResponse), 200)]
    public override async Task<ActionResult<CutListTemplateGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new CutListTemplateGetAllQuery(), cancellationToken));
    }
}