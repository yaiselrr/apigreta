using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;
using Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;
[Route("api/MixAndMatch")]
public class MixAndMatchUpdate: EndpointBaseAsync.WithRequest<MixAndMatchUpdateRequest>.WithResult<MixAndMatchUpdateResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a MixAndMatch by Id",
        Description = "Update a MixAndMatch by Id",
        OperationId = "MixAndMatch.Update",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchGetByIdResponse), 200)]
    public override async Task<MixAndMatchUpdateResponse> HandleAsync(
        [FromMultiSource] MixAndMatchUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new MixAndMatchUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}