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
/// <param name="State"></param>
public record ProfilesChangeStateCommand(long Id, bool State) : IRequest<ProfilesChangeStateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("add_edit_profiles")
    };
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class ProfilesChangeStateHandler : IRequestHandler<ProfilesChangeStateCommand, ProfilesChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ProfilesChangeStateHandler(ILogger<ProfilesChangeStateHandler> logger, IProfilesService service)
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
    public async Task<ProfilesChangeStateResponse> Handle(ProfilesChangeStateCommand request, CancellationToken cancellationToken=default)
    {        
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new ProfilesChangeStateResponse { Data = result};
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record ProfilesChangeStateResponse : CQRSResponse<bool>;
