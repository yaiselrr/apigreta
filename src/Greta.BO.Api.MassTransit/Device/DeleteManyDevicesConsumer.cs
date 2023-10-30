using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.Api.EventContracts.Device;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Device
{
    public class DeleteManyDevicesConsumer : BaseConsumer<DeleteManyDevicesRequestContract>
    {
        private readonly IDeviceService _deviceService;

        public DeleteManyDevicesConsumer(
            ILogger<DeleteManyDevicesConsumer> logger,
            IDeviceService deviceService,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _deviceService = deviceService;
        }

        public override async Task Execute(ConsumeContext<DeleteManyDevicesRequestContract> context)
        {
            List<long> devicesId = new List<long>();
            foreach (var guid in context.Message.DevicesId)
                devicesId.Add(await _deviceService.GetIdByGuid(guid));

            var resultSuccess = await _deviceService.DeleteRange(devicesId);

            if (resultSuccess)
            {
                _logger.LogInformation("Device delete successfully");

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = true,
                    Message = "Device delete successfully",
                    Timestamp = DateTime.Now
                });
            }
            else
            {
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string> {"Error delete device."},
                    Timestamp = DateTime.Now
                });
            }
        }
    }
}