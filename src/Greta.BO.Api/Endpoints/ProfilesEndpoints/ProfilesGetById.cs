using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;

[Route("api/Profiles")]
public class ProfilesGetById : EndpointBaseAsync.WithRequest<ProfilesGetByIdRequest>.WithActionResult<ProfilesGetByIdResponse>
{
    private readonly IMediator _mediator;

    public ProfilesGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Profile by id",
        Description = "Get Profile by id",
        OperationId = "Profiles.GetById",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(ProfilesGetByIdResponse), 200)]
    public override async Task<ActionResult<ProfilesGetByIdResponse>> HandleAsync(
        [FromMultiSource] ProfilesGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ProfilesGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}