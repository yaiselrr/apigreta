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
public class ProfilesFilter: EndpointBaseAsync.WithRequest<ProfilesFilterRequest>.WithActionResult<ProfilesFilterResponse>
{
    private readonly IMediator _mediator;

    public ProfilesFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of profiles entity",
        Description = "Gets a paginated list of profiles entity",
        OperationId = "Profiles.Filter",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(ProfilesFilterResponse), 200)]
    public override async Task<ActionResult<ProfilesFilterResponse>> HandleAsync(
        [FromMultiSource]ProfilesFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ProfilesFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}