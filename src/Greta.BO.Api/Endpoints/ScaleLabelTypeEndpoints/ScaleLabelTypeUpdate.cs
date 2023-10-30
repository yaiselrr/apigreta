using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelType;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;
[Route("api/ScaleLabelType")]
public class ScaleLabelTypeUpdate : EndpointBaseAsync.WithRequest<ScaleLabelTypeUpdateRequest>.WithResult<ScaleLabelTypeUpdateResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a scale label type entity by Id",
        Description = "Update a scale label type entity by Id",
        OperationId = "ScaleLabelType.Update",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeGetByIdResponse), 200)]
    public override async Task<ScaleLabelTypeUpdateResponse> HandleAsync(
        [FromMultiSource] ScaleLabelTypeUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleLabelTypeUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}