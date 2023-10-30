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
public class RegionCreate: EndpointBaseAsync.WithRequest<RegionCreateRequest>.WithResult<RegionCreateResponse>
{
    private readonly IMediator _mediator;

    public RegionCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new region entity",
        Description = "Create a new region entity",
        OperationId = "Region.Create",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionGetByIdResponse), 200)]
    public override async Task<RegionCreateResponse> HandleAsync(
        [FromMultiSource] RegionCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RegionCreateCommand(request.EntityDto), cancellationToken);
    }
}