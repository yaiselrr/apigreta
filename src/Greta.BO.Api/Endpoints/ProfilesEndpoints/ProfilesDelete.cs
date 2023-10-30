using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;
[Route("api/Profiles")]
public class ProfilesDelete : EndpointBaseAsync.WithRequest<ProfilesDeleteRequest>.WithActionResult<ProfilesDeleteResponse>
{
    private readonly IMediator _mediator;

    public ProfilesDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Profile by Id",
        Description = "Delete a Profile by Id",
        OperationId = "Profiles.Delete",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(ProfilesDeleteResponse), 200)]
    public override async Task<ActionResult<ProfilesDeleteResponse>> HandleAsync(
        [FromMultiSource] ProfilesDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ProfilesDeleteCommand(request.Id), cancellationToken);
    }
}