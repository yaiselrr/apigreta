using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Region;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;

[Route("api/Region")]
public class RegionGetById : EndpointBaseAsync.WithRequest<RegionGetByIdRequest>.WithActionResult<RegionGetByIdResponse>
{
    private readonly IMediator _mediator;

    public RegionGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get region entity by id",
        Description = "Get region entity by id",
        OperationId = "Region.GetById",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionGetByIdResponse), 200)]
    public override async Task<ActionResult<RegionGetByIdResponse>> HandleAsync(
        [FromMultiSource] RegionGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new RegionGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}