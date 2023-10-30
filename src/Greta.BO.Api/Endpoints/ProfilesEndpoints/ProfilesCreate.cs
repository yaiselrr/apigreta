using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.Api.Endpoints.FamilyEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using Greta.BO.BusinessLogic.Handlers.Queries.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;
[Route("api/Profiles")]
public class ProfilesCreate: EndpointBaseAsync.WithRequest<ProfilesCreateRequest>.WithResult<ProfilesCreateResponse>
{
    private readonly IMediator _mediator;

    public ProfilesCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Profiles",
        Description = "Create a new Profiles",
        OperationId = "Profiles.Create",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(ProfilesGetByIdResponse), 200)]
    public override async Task<ProfilesCreateResponse> HandleAsync(
        [FromMultiSource] ProfilesCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ProfilesCreateCommand(request.EntityDto), cancellationToken);
    }
}