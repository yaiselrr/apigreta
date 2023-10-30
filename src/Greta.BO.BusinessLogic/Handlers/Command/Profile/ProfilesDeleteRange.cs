using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Profile;

/// <summary>
/// </summary>
/// <param name="Ids"></param>
public record ProfilesDeleteRangeCommand(List<long> Ids) : IRequest<ProfilesDeleteRangeResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("delete_profiles")
    };
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class ProfilesDeleteRangeHandler : IRequestHandler<ProfilesDeleteRangeCommand, ProfilesDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ProfilesDeleteRangeHandler(
        ILogger<ProfilesDeleteRangeHandler> logger,
        IProfilesService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProfilesDeleteRangeResponse> Handle(ProfilesDeleteRangeCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new ProfilesDeleteRangeResponse { Data = result};
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record ProfilesDeleteRangeResponse : CQRSResponse<bool>;
