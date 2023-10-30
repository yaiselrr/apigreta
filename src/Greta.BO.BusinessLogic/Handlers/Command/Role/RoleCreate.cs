// Ignore Spelling: Validator

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Role;

/// <summary>
/// Query for create new rol
/// </summary>
/// <param name="Entity">rol entity</param>
public record RoleCreateCommand(RoleModel Entity) : IRequest<RoleCreateResponse>, IAuthorizable
{
    ///<inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement(string.Format("add_edit_{0}",nameof(Role).ToLower()))
    };
}

///<inheritdoc/>
public class RoleCreateValidator : AbstractValidator<RoleCreateCommand>
{
    private readonly IRoleService _service;

    ///<inheritdoc/>
    public RoleCreateValidator(IRoleService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Role name already exists");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var rolExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Role>(name), cancellationToken);
        return rolExist == null;
    }
}

///<inheritdoc/>
public class RoleCreateHandler : IRequestHandler<RoleCreateCommand, RoleCreateResponse>
{
    private readonly ILogger _logger;

    private readonly IMapper _mapper;

    private readonly IRoleService _service;

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoleCreateHandler(
        ILogger<RoleCreateHandler> logger,
        IRoleService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    ///<inheritdoc/>
    public async Task<RoleCreateResponse> Handle(RoleCreateCommand request, CancellationToken cancellationToken=default)
    {
        var entity = _mapper.Map<Api.Entities.Role>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Role {ResultName} for user {ResultUserCreatorId}", result.Name, result.UserCreatorId);
        return new RoleCreateResponse { Data = _mapper.Map<RoleModel>(result)};
    }
}

///<inheritdoc/>
public record RoleCreateResponse : CQRSResponse<RoleModel>;
