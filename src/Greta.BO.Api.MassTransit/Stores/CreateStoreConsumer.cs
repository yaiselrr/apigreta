using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.EventContracts.Store;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Stores
{
    public class CreateStoreConsumer : BaseConsumer<CreateStoreRequestContract>
    {
        private readonly IStoreRepository _storeRepository;

        public CreateStoreConsumer(
            ILogger<CreateStoreConsumer> logger,
            IStoreRepository storeRepository,
            IOptions<MainOption> options
        )
            : base(logger, options)
        {
            _storeRepository = storeRepository;
        }

        public override async Task Execute(ConsumeContext<CreateStoreRequestContract> context)
        {
            var now = DateTime.Now;
            var defaultOpenTimedefaultOpenTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
            var defaultCloseTime = new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);

            //create store
            Store store = new Store
            {
                Name = context.Message.Name,
                RegionId = 1,
                UserCreatorId = userId,
                GuidId = context.Message.GuidId,
                OpenTime = defaultOpenTimedefaultOpenTime,
                ClosedTime = defaultCloseTime,
                TimeZoneId = "UTC",
                State = true,
            };
            var bo_store = await _storeRepository.CreateAsync<Store>(store);


            if (bo_store != null)
            {
                _logger.LogInformation("Store creation successfully");

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = true,
                    Message = "Store creation successfully",
                    Timestamp = DateTime.Now
                });
            }
            else
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string>() {"Error create store."},
                    Timestamp = DateTime.Now,
                });
        }
    }
}