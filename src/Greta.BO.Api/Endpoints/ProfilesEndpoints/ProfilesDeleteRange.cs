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
public class ProfilesDeleteRange: EndpointBaseAsync.WithRequest<ProfilesDeleteRangeRequest>.WithResult<ProfilesDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public ProfilesDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Profiles",
        Description = "Delete list of Profiles",
        OperationId = "Profiles.DeleteRange",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(ProfilesDeleteRangeResponse), 200)]
    public override async Task<ProfilesDeleteRangeResponse> HandleAsync(
        [FromMultiSource] ProfilesDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ProfilesDeleteRangeCommand(request.Ids), cancellationToken);
    }
}