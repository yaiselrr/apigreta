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
public class CutListTemplateDelete : EndpointBaseAsync.WithRequest<CutListTemplateDeleteRequest>.WithActionResult<CutListTemplateDeleteResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a CutListTemplate by Id",
        Description = "Delete a CutListTemplate by Id",
        OperationId = "CutListTemplate.Delete",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateDeleteResponse), 200)]
    public override async Task<ActionResult<CutListTemplateDeleteResponse>> HandleAsync(
        [FromMultiSource] CutListTemplateDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListTemplateDeleteCommand(request.Id), cancellationToken);
    }
}