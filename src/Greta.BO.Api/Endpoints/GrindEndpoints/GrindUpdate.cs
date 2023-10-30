using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Grind;
using Greta.BO.BusinessLogic.Handlers.Queries.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;
[Route("api/Grind")]
public class GrindUpdate : EndpointBaseAsync.WithRequest<GrindUpdateRequest>.WithResult<GrindUpdateResponse>
{
    private readonly IMediator _mediator;

    public GrindUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Grind entity by Id",
        Description = "Update a Grind entity by Id",
        OperationId = "Grind.Update",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindGetByIdResponse), 200)]
    public override async Task<GrindUpdateResponse> HandleAsync(
        [FromMultiSource] GrindUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GrindUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}