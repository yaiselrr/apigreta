using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Device;

/// <summary>
/// Update entity
/// </summary>
/// <param name="DeviceId">Entity id</param>
/// <param name="NewTag">New entity</param>
public record DeviceUpdateTagVersionCommand(string DeviceId, long NewTag) : IRequest<bool>;

/// <inheritdoc />
public class DeviceUpdateTagVersionHandler : IRequestHandler<DeviceUpdateTagVersionCommand, bool>
{
    private readonly ILogger _logger;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DeviceUpdateTagVersionHandler(
        ILogger<DeviceUpdateTagVersionHandler> logger,
        IDeviceService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<bool> Handle(DeviceUpdateTagVersionCommand request, CancellationToken cancellationToken)
    {
        var success = await _service.UpdateTagVersion(request.DeviceId, request.NewTag);
        _logger.LogInformation("Device with deviceId {DeviceId} update to tag version {NewTag} update successfully ",request.DeviceId, request.NewTag);
        return success;
    }
}