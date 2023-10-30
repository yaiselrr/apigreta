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
/// 
/// </summary>
/// <param name="Id"></param>
public record ProfilesDeleteCommand(long Id) : IRequest<ProfilesDeleteResponse>, IAuthorizable
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
public class ProfilesDeleteHandler : IRequestHandler<ProfilesDeleteCommand, ProfilesDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ProfilesDeleteHandler(ILogger<ProfilesDeleteHandler> logger, IProfilesService service)
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
    public async Task<ProfilesDeleteResponse> Handle(ProfilesDeleteCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new ProfilesDeleteResponse { Data = result};
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record ProfilesDeleteResponse : CQRSResponse<bool>;
