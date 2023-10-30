using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Region;

/// <summary>
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record RegionChangeStateCommand(long Id, bool State) : IRequest<RegionChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Region).ToLower()}")
    };
}

/// <inheritdoc />
public class RegionChangeStateHandler : IRequestHandler<RegionChangeStateCommand, RegionChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IRegionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RegionChangeStateHandler(ILogger<RegionChangeStateHandler> logger, IRegionService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<RegionChangeStateResponse> Handle(RegionChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new RegionChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record RegionChangeStateResponse : CQRSResponse<bool>;