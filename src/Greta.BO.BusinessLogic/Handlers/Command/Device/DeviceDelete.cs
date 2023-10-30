using System.Collections.Generic;
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
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record DeviceDeleteCommand(long Id) : IRequest<DeviceDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Device).ToLower()}")
    };
}

/// <inheritdoc />
public class DeviceDeleteHandler : IRequestHandler<DeviceDeleteCommand, DeviceDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DeviceDeleteHandler(
        ILogger<DeviceDeleteHandler> logger,
        IDeviceService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<DeviceDeleteResponse> Handle(DeviceDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new DeviceDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record DeviceDeleteResponse : CQRSResponse<bool>;