using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Scalendar;
using Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScalendarEndpoints;
[Route("api/Scalendar")]
public class ScalendarUpdate : EndpointBaseAsync.WithRequest<ScalendarUpdateRequest>.WithResult<ScalendarUpdateResponse>
{
    private readonly IMediator _mediator;

    public ScalendarUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Scalendar entity by Id",
        Description = "Update a Scalendar entity by Id",
        OperationId = "Scalendar.Update",
        Tags = new[] { "Scalendar" })
    ]
    [ProducesResponseType(typeof(ScalendarGetByIdResponse), 200)]
    public override async Task<ScalendarUpdateResponse> HandleAsync(
        [FromMultiSource] ScalendarUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScalendarUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}