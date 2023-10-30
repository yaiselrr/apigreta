using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Role;

/// <summary>
/// Query for change state of rol
/// </summary>
/// <param name="Id">id of rol</param>
/// <param name="State">bool state</param>
public record RoleChangeStateCommand(long Id, bool State) : IRequest<RoleChangeStateResponse>, IAuthorizable
{
    ///<inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement(string.Format("add_edit_{0}",nameof(Role).ToLower()))
    };
}

///<inheritdoc/>
public class RoleChangeStateHandler : IRequestHandler<RoleChangeStateCommand, RoleChangeStateResponse>
{
    private readonly ILogger _logger;

    private readonly IRoleService _service;

  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RoleChangeStateHandler(ILogger<RoleChangeStateHandler> logger, IRoleService service)
    {
        _logger = logger;
        _service = service;
    }

    ///<inheritdoc/>
    public async Task<RoleChangeStateResponse> Handle(RoleChangeStateCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new RoleChangeStateResponse { Data = result};
    }
}

///<inheritdoc/>
public record RoleChangeStateResponse : CQRSResponse<bool>;
