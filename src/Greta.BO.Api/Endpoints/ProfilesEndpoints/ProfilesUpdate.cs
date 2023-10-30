using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using Greta.BO.BusinessLogic.Handlers.Queries.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;
[Route("api/Profiles")]
public class ProfilesUpdate: EndpointBaseAsync.WithRequest<ProfilesUpdateRequest>.WithResult<ProfilesUpdateResponse>
{
    private readonly IMediator _mediator;

    public ProfilesUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Profiles by Id",
        Description = "Update a Profiles by Id",
        OperationId = "Profiles.Update",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(ProfilesGetByIdResponse), 200)]
    public override async Task<ProfilesUpdateResponse> HandleAsync(
        [FromMultiSource] ProfilesUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ProfilesUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}