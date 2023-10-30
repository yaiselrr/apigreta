using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;
[Route("api/Family")]
public class FamilyDeleteRange: EndpointBaseAsync.WithRequest<FamilyDeleteRangeRequest>.WithResult<FamilyDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public FamilyDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Family",
        Description = "Delete list of Family",
        OperationId = "Family.DeleteRange",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyDeleteRangeResponse), 200)]
    public override async Task<FamilyDeleteRangeResponse> HandleAsync(
        [FromMultiSource] FamilyDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FamilyDeleteRangeCommand(request.Ids), cancellationToken);
    }
}