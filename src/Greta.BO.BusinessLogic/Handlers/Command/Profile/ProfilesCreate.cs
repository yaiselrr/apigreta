using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Profile;

/// <summary>
/// 
/// </summary>
/// <param name="Entity"></param>
public record ProfilesCreateCommand(ProfilesModel Entity) : IRequest<ProfilesCreateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_profiles")
    };
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class ProfilesCreateValidator : AbstractValidator<ProfilesCreateCommand>
{
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    public ProfilesCreateValidator(IProfilesService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Profile name already exists");
    }

    /// <summary>
    /// Check unique name of profiles
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {       
        var profileExist = await _service.Get(new CheckUniqueNameSpec<Profiles>(name), cancellationToken);
        return profileExist == null;
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class ProfilesCreateHandler : IRequestHandler<ProfilesCreateCommand, ProfilesCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    public ProfilesCreateHandler(
        ILogger<ProfilesCreateHandler> logger,
        IProfilesService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProfilesCreateResponse> Handle(ProfilesCreateCommand request, CancellationToken cancellationToken=default)
    {
        var entity = _mapper.Map<Profiles>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Profile {ResultName} for user {ResultUserCreatorId}", result.Name, result.UserCreatorId);
        return new ProfilesCreateResponse { Data = _mapper.Map<ProfilesListModel>(result)};
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record ProfilesCreateResponse : CQRSResponse<ProfilesListModel>;
