using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

[Route("api/Family")]
public class FamilyGetById : EndpointBaseAsync.WithRequest<FamilyGetByIdRequest>.WithActionResult<FamilyGetByIdResponse>
{
    private readonly IMediator _mediator;

    public FamilyGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by id",
        Description = "Get Entity by id",
        OperationId = "Family.GetById",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyGetByIdResponse), 200)]
    public override async Task<ActionResult<FamilyGetByIdResponse>> HandleAsync(
        [FromMultiSource] FamilyGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new FamilyGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}