using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.Api.EventContracts.Device;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Device
{
    public class GetFileFromDeviceConsumer: BaseConsumer<GetFileFromDeviceRequestContract>
    {
        private readonly IDeviceService _deviceService;
        private readonly IHubContext<CloudHub, ICloudHub> _cloudHubContext;
        
        public GetFileFromDeviceConsumer(
            ILogger<GetFileFromDeviceConsumer> logger, 
            IOptions<MainOption> options,
            IDeviceService deviceService,
            IHubContext<CloudHub, ICloudHub> cloudHubContext
        ) : base(logger, options)
        {
            _deviceService = deviceService;
            _cloudHubContext = cloudHubContext;
        }

        public override async Task Execute(ConsumeContext<GetFileFromDeviceRequestContract> context)
        {
            var deviceGuid = context.Message.DeviceGuid;
            var device = await _deviceService.GetConnectionIdByGuid(deviceGuid);
            if (device == null)
            {
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string> {"Error resolving device."},
                    Timestamp = DateTime.Now
                });
                return;
            }
            await _cloudHubContext.Clients.Client(device).CanGetDirectoryFiles(context.Message.Path);
            await context.RespondAsync<BooleanResponseContract>(new
            {
                Status = true,
                Message = "Request sended",
                Timestamp = DateTime.Now
            });
        }
    }
}