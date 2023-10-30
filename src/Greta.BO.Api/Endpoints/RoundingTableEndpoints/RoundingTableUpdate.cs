using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.RoundingTable;
using Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoundingTableEndpoints;
[Route("api/RoundingTable")]
public class RoundingTableUpdate: EndpointBaseAsync.WithRequest<RoundingTableUpdateRequest>.WithResult<RoundingTableUpdateResponse>
{
    private readonly IMediator _mediator;

    public RoundingTableUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Rounding Table by Id",
        Description = "Update a Rounding Table by Id",
        OperationId = "RoundingTable.Update",
        Tags = new[] { "RoundingTable" })
    ]
    [ProducesResponseType(typeof(RoundingTableGetByIdResponse), 200)]
    public override async Task<RoundingTableUpdateResponse> HandleAsync(
        [FromMultiSource] RoundingTableUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RoundingTableUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}