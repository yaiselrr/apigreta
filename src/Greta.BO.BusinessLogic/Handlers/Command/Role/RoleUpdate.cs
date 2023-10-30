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
/// Query for update rol
/// </summary>
/// <param name="Id">id of rol</param>
/// <param name="Entity">entity rol</param>
public record RoleUpdateCommand(long Id, RoleModel Entity) : IRequest<RoleUpdateResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement(string.Format("add_edit_{0}",nameof(Role).ToLower()))
    };
}

/// <inheritdoc/>
public class RoleUpdateValidator : AbstractValidator<RoleUpdateCommand>
{
    private readonly IRoleService _service;

    /// <inheritdoc/>
    public RoleUpdateValidator(IRoleService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Role name already exists");
    }

    private async Task<bool> NameUnique(RoleUpdateCommand comand, string name, CancellationToken cancellationToken)
    {
        var roleExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Role>(name, comand.Id), cancellationToken);
        return roleExist == null;
    }
}

/// <inheritdoc/>
public class RoleUpdateHandler : IRequestHandler<RoleUpdateCommand, RoleUpdateResponse>
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
    public RoleUpdateHandler(
        ILogger<RoleUpdateHandler> logger,
        IRoleService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<RoleUpdateResponse> Handle(RoleUpdateCommand request, CancellationToken cancellationToken=default)
    {
        var entity = _mapper.Map<Api.Entities.Role>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Role {RequestId} update successfully", request.Id);
        return new RoleUpdateResponse { Data = success};
    }
}

/// <inheritdoc/>
public record RoleUpdateResponse : CQRSResponse<bool>;
