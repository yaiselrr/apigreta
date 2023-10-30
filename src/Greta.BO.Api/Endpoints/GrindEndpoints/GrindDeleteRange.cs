using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;
[Route("api/Grind")]
public class GrindDeleteRange: EndpointBaseAsync.WithRequest<GrindDeleteRangeRequest>.WithResult<GrindDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public GrindDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Grind entities",
        Description = "Delete list of Grind entities",
        OperationId = "Grind.DeleteRange",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindDeleteRangeResponse), 200)]
    public override async Task<GrindDeleteRangeResponse> HandleAsync(
        [FromMultiSource] GrindDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GrindDeleteRangeCommand(request.Ids), cancellationToken);
    }
}