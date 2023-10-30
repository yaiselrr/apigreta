using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Store;
using Greta.BO.BusinessLogic.Handlers.Command.DeviceConfiguration;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Stores
{
    public class PutStoreConfigurationConsumer : BaseConsumer<PutStoreConfigurationRequestContract>
    {
        private readonly IStoreService _service;
        private readonly IMediator _mediator;

        public PutStoreConfigurationConsumer(
            ILogger<PutStoreConfigurationConsumer> logger,
            IOptions<MainOption> options,
            IStoreService service,
            IMediator mediator)
            : base(logger, options)
        {
            _service = service;
            _mediator = mediator;
        }

        public override async Task Execute(ConsumeContext<PutStoreConfigurationRequestContract> context)
        {
            
            var store = await _service.GetById(context.Message
                .Id); //await _mediator.Send(new StoreGetById.Query(context.Message.Id));
            
            if (store != null)
            {
                _logger.LogInformation("Init store configuration for store {Name}", store.Name);
                var entity = context.Message;
                var properties = typeof(PutStoreConfigurationRequestContract).GetProperties().ToList();

                foreach (var property in properties)
                {
                    var value = entity.GetType().GetProperty(property.Name)?.GetValue(entity);

                    if (value != null)
                        store.GetType().GetProperty(property.Name)?.SetValue(store, value, null);
                }

                var success = await _service.PutConfiguration(store.Id, store);
                if (success)
                {
                    await _mediator.Send(new SendToDeviceCommand(store.Id, null));

                    await context.RespondAsync<BooleanResponseContract>(new
                    {
                        Status = true,
                        Message = "Store configuration successfully saved.",
                        Timestamp = DateTime.Now
                    });
                    return;
                }
            }

            await context.RespondAsync<FailResponseContract>(new
            {
                errorMessages = new List<string>()
                    {$"Error setting store configuration for store id {context.Message.Id}."},
                Timestamp = DateTime.Now,
            });
        }
    }
}