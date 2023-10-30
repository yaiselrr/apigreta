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
/// Query for delete rol by id
/// </summary>
/// <param name="Id">id of rol</param>
public record RoleDeleteCommand(long Id) : IRequest<RoleDeleteResponse>, IAuthorizable
{
    ///<inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement(string.Format("delete_{0}",nameof(Role).ToLower()))
    };
}

///<inheritdoc/>
public class RoleDeleteHandler : IRequestHandler<RoleDeleteCommand, RoleDeleteResponse>
{    
    private readonly ILogger _logger;    

    private readonly IRoleService _service;

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RoleDeleteHandler(ILogger<RoleDeleteHandler> logger, IRoleService service)
    {
        _logger = logger;
        _service = service;
    }

    ///<inheritdoc/>
    public async Task<RoleDeleteResponse> Handle(RoleDeleteCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new RoleDeleteResponse { Data = result};
    }
}

///<inheritdoc/>
public record RoleDeleteResponse : CQRSResponse<bool>;
