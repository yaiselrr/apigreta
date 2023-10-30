using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;
using Greta.BO.BusinessLogic.Handlers.Queries.ExternalScale;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;
[Route("api/ExternalScale")]
public class ExternalScaleUpdate : EndpointBaseAsync.WithRequest<ExternalScaleUpdateRequest>.WithResult<ExternalScaleUpdateResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a external scale entity by Id",
        Description = "Update a external scale entity by Id",
        OperationId = "ExternalScale.Update",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleGetByIdResponse), 200)]
    public override async Task<ExternalScaleUpdateResponse> HandleAsync(
        [FromMultiSource] ExternalScaleUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ExternalScaleUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}