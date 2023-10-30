using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Profiles;

/// <summary>
/// 
/// </summary>
public record ProfilesGetByApplicationQuery(long ApplicationId) : IRequest<ProfilesGetByApplicationResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Profiles).ToLower()}")
    };
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class ProfilesGetByApplicationHandler : IRequestHandler<ProfilesGetByApplicationQuery, ProfilesGetByApplicationResponse>
{
    private readonly IMapper _mapper;
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    public ProfilesGetByApplicationHandler(IProfilesService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Get profiles by Application
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProfilesGetByApplicationResponse> Handle(ProfilesGetByApplicationQuery request, CancellationToken cancellationToken=default)
    {
        var entity = await _service.GetByApplication(request.ApplicationId);
        var profilesByApplication = new List<Api.Entities.Profiles>() {new Api.Entities.Profiles() {Id = -1, Name = "Not used"}};
        profilesByApplication.AddRange(entity);
        return new ProfilesGetByApplicationResponse { Data = _mapper.Map<List<ProfilesModel>>(profilesByApplication) };
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record ProfilesGetByApplicationResponse : CQRSResponse<List<ProfilesModel>>;
