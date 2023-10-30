using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Synchro;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Hangfire.MediatR;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Device;

/// <summary>
/// Device force full synchro
/// </summary>
/// <param name="DeviceId">Device id</param>
public record DeviceForceFullSynchroCommand(long DeviceId): IRequest<DeviceForceFullSynchroResponse>;

/// <inheritdoc />
public record DeviceForceFullSynchroResponse: CQRSResponse<bool>;

/// <inheritdoc />
public class DeviceForceFullSynchroHandler: IRequestHandler<DeviceForceFullSynchroCommand, DeviceForceFullSynchroResponse>
{
    private readonly ILogger<DeviceForceFullSynchroHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IDeviceService _deviceService;
    private readonly IStoreService _storeService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    /// <param name="deviceService"></param>
    /// <param name="storeService"></param>
    public DeviceForceFullSynchroHandler(
        ILogger<DeviceForceFullSynchroHandler> logger,
        IMediator mediator,
        IDeviceService deviceService,
        IStoreService storeService)
    {
        _logger = logger;
        _mediator = mediator;
        _deviceService = deviceService;
        _storeService = storeService;
    }
    
    /// <inheritdoc />
    public async Task<DeviceForceFullSynchroResponse> Handle(DeviceForceFullSynchroCommand request, CancellationToken cancellationToken)
    {
        var device = await _deviceService.Get(request.DeviceId);

        if (device == null)
        {
            _logger.LogError("Device with deviceId {DeviceId} not found ", request.DeviceId);
            throw new BussinessValidationException($"Device with deviceId {request.DeviceId} not found ");
        }
        _logger.LogInformation("Requesting a full backup for device {DeviceName}", device.Name);

        var store = await _storeService.Get(device.StoreId);

        if (store == null)
        {
            _logger.LogError("Store with StoreId {StoreId} not found ", device.StoreId);
            throw new BussinessValidationException($"Store with storeId {device.StoreId} not found ");
        }
        _mediator.EnqueueNew(new SynchroFullBackupCommand(store.GuidId, ConnectionId: device.SignalRConnectionId));

        return new DeviceForceFullSynchroResponse() { Data = true };
    }
}