using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Device;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Device
{
    public class CreateManyDevicesConsumer : BaseConsumer<CreateManyDevicesRequestContract>
    {
        private readonly IDeviceRepository _DeviceRepository;
        private readonly IStoreRepository _StoreRepository;

        public CreateManyDevicesConsumer(
            ILogger<CreateManyDevicesConsumer> logger,
            IDeviceRepository DeviceRepository,
            IStoreRepository StoreRepository ,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _DeviceRepository = DeviceRepository;
            _StoreRepository = StoreRepository;
        }

        public override async Task Execute(ConsumeContext<CreateManyDevicesRequestContract> context)
        {
            var storeId = await _StoreRepository.GetEntity<Entities.Store>()
                //.Where(x => x.GuidId == context.Message.StoreGuidId)
                .Where(x => x.GuidId == context.Message.StoreGuidId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            //.FirstOrDefaultAsync(e => e.GuidId == context.Message.StoreGuidId);
            List<Entities.Device> devices = new List<Entities.Device>();
            for (int i = 0; i < context.Message.Devices.Count; i++)
            {
                devices.Add(new Entities.Device
                {
                    Name = context.Message.Devices.ElementAt(i).Key,
                    GuidId = context.Message.Devices.ElementAt(i).Value,
                    ScaleComName = "COM3",
                    ScaleBaudRate = 9600,
                    UserCreatorId = userId,
                    StoreId = storeId,
                    State = true
                });
            }

            // si la tienda no existe no se puede agregar un dispositivo en una tienda
            var result = await _DeviceRepository.CreateRangeAsync<Entities.Device>(devices);

            if (result != null)
            {
                _logger.LogInformation("Device creation successfully");

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = true,
                    Message = "Device Create successfully in BO.",
                    Timestamp = DateTime.Now
                });
            }
            else
            {
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string> {"Error create device."},
                    Timestamp = DateTime.Now
                });
            }
        }
    }
}