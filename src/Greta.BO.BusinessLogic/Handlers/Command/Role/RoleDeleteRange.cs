using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Role;

/// <summary>
/// Query for delete range of rol
/// </summary>
/// <param name="Ids">List of ids</param>
public record RoleDeleteRangeCommand(List<long> Ids) : IRequest<RoleDeleteRangeResponse>, IAuthorizable
{
    ///<inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement(string.Format("delete_{0}",nameof(Role).ToLower()))
    };
}
///<inheritdoc/>
public class RoleDeleteRangeHandler : IRequestHandler<RoleDeleteRangeCommand, RoleDeleteRangeResponse>
{
    private readonly ILogger _logger;

    private readonly IRoleService _service;

   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RoleDeleteRangeHandler(
        ILogger<RoleDeleteRangeHandler> logger,
        IRoleService service)
    {
        _logger = logger;
        _service = service;
    }

    ///<inheritdoc/>
    public async Task<RoleDeleteRangeResponse> Handle(RoleDeleteRangeCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new RoleDeleteRangeResponse { Data = result};
    }
}

///<inheritdoc/>
public record RoleDeleteRangeResponse : CQRSResponse<bool>;
