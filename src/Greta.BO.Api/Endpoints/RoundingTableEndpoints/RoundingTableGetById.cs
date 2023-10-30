using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoundingTableEndpoints;

[Route("api/RoundingTable")]
public class RoundingTableGetById : EndpointBaseAsync.WithRequest<RoundingTableGetByIdRequest>.WithActionResult<RoundingTableGetByIdResponse>
{
    private readonly IMediator _mediator;

    public RoundingTableGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by id",
        Description = "Get Entity by id",
        OperationId = "RoundingTable.GetById",
        Tags = new[] { "RoundingTable" })
    ]
    [ProducesResponseType(typeof(RoundingTableGetByIdResponse), 200)]
    public override async Task<ActionResult<RoundingTableGetByIdResponse>> HandleAsync(
        [FromMultiSource] RoundingTableGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new RoundingTableGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}