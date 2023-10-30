using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Family;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;
[Route("api/Family")]
public class FamilyUpdate: EndpointBaseAsync.WithRequest<FamilyUpdateRequest>.WithResult<FamilyUpdateResponse>
{
    private readonly IMediator _mediator;

    public FamilyUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Family by Id",
        Description = "Update a Family by Id",
        OperationId = "Family.Update",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyGetByIdResponse), 200)]
    public override async Task<FamilyUpdateResponse> HandleAsync(
        [FromMultiSource] FamilyUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FamilyUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}