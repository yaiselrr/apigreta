using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Region;
using Greta.BO.BusinessLogic.Handlers.Queries.Region;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;
[Route("api/Region")]
public class RegionUpdate : EndpointBaseAsync.WithRequest<RegionUpdateRequest>.WithResult<RegionUpdateResponse>
{
    private readonly IMediator _mediator;

    public RegionUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a region entity by Id",
        Description = "Update a region entity by Id",
        OperationId = "Region.Update",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionGetByIdResponse), 200)]
    public override async Task<RegionUpdateResponse> HandleAsync(
        [FromMultiSource] RegionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RegionUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}