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
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record ProfilesUpdateCommand(long Id, ProfilesModel Entity) : IRequest<ProfilesUpdateResponse>, IAuthorizable
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
public class ProfilesUpdateValidator : AbstractValidator<ProfilesUpdateCommand>
{
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public ProfilesUpdateValidator(IProfilesService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Profile name already exists");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<bool> NameUnique(ProfilesUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var profilesExist = await _service.Get(new CheckUniqueNameSpec<Profiles>(name, command.Id), cancellationToken);
        return profilesExist == null;
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class ProfilesUpdateHandler : IRequestHandler<ProfilesUpdateCommand, ProfilesUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ProfilesUpdateHandler(
        ILogger<ProfilesUpdateHandler> logger,
        IProfilesService service, IMapper mapper)
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
    public async Task<ProfilesUpdateResponse> Handle(ProfilesUpdateCommand request, CancellationToken cancellationToken=default)
    {
        var entity = _mapper.Map<Profiles>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Profile {RequestId} update successfully", request.Id);
        return new ProfilesUpdateResponse { Data = success};
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record ProfilesUpdateResponse : CQRSResponse<bool>;
