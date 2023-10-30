using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Device;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record DeviceDeleteRangeCommand(List<long> Ids) : IRequest<DeviceDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Device).ToLower()}")
    };
}

/// <inheritdoc />
public class DeviceDeleteRangeHandler : IRequestHandler<DeviceDeleteRangeCommand, DeviceDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DeviceDeleteRangeHandler(
        ILogger<DeviceDeleteRangeHandler> logger,
        IDeviceService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<DeviceDeleteRangeResponse> Handle(DeviceDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new DeviceDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record DeviceDeleteRangeResponse : CQRSResponse<bool>;