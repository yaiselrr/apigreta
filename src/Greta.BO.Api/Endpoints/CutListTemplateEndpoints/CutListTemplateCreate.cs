using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;
[Route("api/CutListTemplate")]
public class CutListTemplateCreate : EndpointBaseAsync.WithRequest<CutListTemplateCreateRequest>.WithActionResult<CutListTemplateCreateResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateCreate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new CutListTemplate",
        Description = "Create a new CutListTemplate",
        OperationId = "CutListTemplate.Create",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateCreateResponse), 201)]
    public override async Task<ActionResult<CutListTemplateCreateResponse>> HandleAsync(
        [FromMultiSource] CutListTemplateCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new CutListTemplateCreateCommand(request.EntityDto), cancellationToken);
        return result.Data ? Created(string.Empty, result): BadRequest();
    }
}