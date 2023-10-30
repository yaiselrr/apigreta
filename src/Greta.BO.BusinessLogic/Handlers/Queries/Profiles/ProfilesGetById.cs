using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Profiles;
/// <summary>
/// Query for get profile by Id
/// </summary>
/// <param name="Id"></param>
public record ProfilesGetByIdQuery(long Id) : IRequest<ProfilesGetByIdResponse>, IAuthorizable //, ICacheable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new() {
        new PermissionRequirement.Requirement($"view_{nameof(Profiles).ToLower()}")
    };
}

/// <inheritdoc/>
public class ProfilesGetByIdHandler : IRequestHandler<ProfilesGetByIdQuery, ProfilesGetByIdResponse>
{
    private readonly ILogger _logger;
    private readonly IProfilesService _service;
    /// <summary>
    /// Constructor of ProfilesGetByIdHandler
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public ProfilesGetByIdHandler(ILogger<ProfilesGetByIdHandler> logger,IProfilesService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Handler of ProfilesGetByIdHandler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProfilesGetByIdResponse> Handle(ProfilesGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var spec = new ProfilesGetByIdSpec(request.Id);
        var data = await _service.Get(spec, cancellationToken);
        return new ProfilesGetByIdResponse { Data = data.SingleOrDefault() };
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record ProfilesGetByIdResponse : CQRSResponse<Api.Entities.Profiles>;
    
