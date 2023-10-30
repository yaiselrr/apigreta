using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.EventContracts.Device;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Device
{
    public class CreateDeviceConsumer : BaseConsumer<BOApiCreateDeviceRequestContract>
    {
        private readonly IDeviceRepository _DeviceRepository;
        private readonly IStoreRepository _StoreRepository;

        public CreateDeviceConsumer(
            ILogger<CreateDeviceConsumer> logger, 
            IDeviceRepository DeviceRepository,
            IStoreRepository StoreRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _DeviceRepository = DeviceRepository;
            _StoreRepository = StoreRepository;
        }

        //TODO we need notify to add min this new device on imail or on dashboard notification
        public override async Task Execute(ConsumeContext<BOApiCreateDeviceRequestContract> context)
        {
            var store = await _StoreRepository.GetEntity<Store>()
                //.Where(x => x.GuidId == context.Message.StoreGuidId)
                .Where(x => x.GuidId == context.Message.StoreGuidId)
                .Select(x => new {Id = x.Id, Name = x.Name})
                .FirstOrDefaultAsync();
            if (store == null)
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string> {"Store not found."},
                    Timestamp = DateTime.Now
                });
            //.FirstOrDefaultAsync(e => e.GuidId == context.Message.StoreGuidId);
            var device = (new Entities.Device
            {
                Name = context.Message.Name,
                GuidId = context.Message.GuidId,
                DeviceId = context.Message.LicenseCode,
                UserCreatorId = userId,
                StoreId = store.Id,
                State = true
            });

            // si la tienda no existe no se puede agregar un dispositivo en una tienda
            var result = await _DeviceRepository.CreateAsync<Entities.Device>(device);

            if (result != null)
            {
                _logger.LogInformation($"Device {device.Name} for store {store.Name}, creation successfully");

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