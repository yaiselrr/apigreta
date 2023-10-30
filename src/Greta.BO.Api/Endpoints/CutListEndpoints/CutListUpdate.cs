using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

[Route("api/CutList")]
public class CutListUpdate : EndpointBaseAsync.WithRequest<CutListUpdateRequest>.WithResult<CutListUpdateResponse>
{
    private readonly IMediator _mediator;

    public CutListUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a CutList entity by Id",
        Description = "Update a CutList entity by Id",
        OperationId = "CutList.Update",
        Tags = new[] { "CutList" })
    ]
    [ProducesResponseType(typeof(CutListUpdateResponse), 200)]
    public override async Task<CutListUpdateResponse> HandleAsync(
        [FromMultiSource] CutListUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}