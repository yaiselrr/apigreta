using System;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Greta.BO.BusinessLogic.Handlers.Command.Device;

public record SendActionCommand
    (long DeviceId, Greta.BO.Api.Entities.Enum.Command Command, string Args) : IRequest<bool>;

public class SendActionHamdler : IRequestHandler<SendActionCommand, bool>
{
    private readonly IDeviceService _deviceService;
    private readonly IHubContext<CloudHub, ICloudHub> _hub;

    public SendActionHamdler(
        IDeviceService deviceService,
        IHubContext<CloudHub, ICloudHub> hub)
    {
        _deviceService = deviceService;
        _hub = hub;
    }

    public async Task<bool> Handle(SendActionCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var device = await _deviceService.Get(request.DeviceId);
            if (device == null || device.SignalRConnectionId == null)
                throw new BussinessValidationException("Device not found or not connected.");

            var clientHub = _hub.Clients.Client(device.SignalRConnectionId);
            
            var result = await clientHub.SingleCommandExecute(
                new SingleCommand()
                {
                    Args = request.Args,
                    Command = request.Command
                }
            );

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}

//