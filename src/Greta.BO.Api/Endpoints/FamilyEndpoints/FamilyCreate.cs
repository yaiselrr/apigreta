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
public class FamilyCreate: EndpointBaseAsync.WithRequest<FamilyCreateRequest>.WithResult<FamilyCreateResponse>
{
    private readonly IMediator _mediator;

    public FamilyCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Family",
        Description = "Create a new Family",
        OperationId = "Family.Create",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyGetByIdResponse), 200)]
    public override async Task<FamilyCreateResponse> HandleAsync(
        [FromMultiSource] FamilyCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FamilyCreateCommand(request.EntityDto), cancellationToken);
    }
}