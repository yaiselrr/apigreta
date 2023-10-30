using System;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.EventContracts.Store;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Stores
{
    public class ChangeStateStoreConsumer : BaseConsumer<BoChangeStateStoreRequestContract>
    {
        protected readonly IStoreRepository _StoreRepository;

        public ChangeStateStoreConsumer(
            ILogger<ChangeStateStoreConsumer> logger,
            IStoreRepository StoreRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _StoreRepository = StoreRepository;
        }

        public override async Task Execute(ConsumeContext<BoChangeStateStoreRequestContract> context)
        {
            this._logger.LogInformation("Start ChangeStatus Store Consumer.");

            //Update Store 
            var store = await _StoreRepository.GetEntity<Store>().FirstOrDefaultAsync(e => e.GuidId == context.Message.GuidId);
            store.State = context.Message.State;

            await _StoreRepository.UpdateAsync(store);

            this._logger.LogInformation("Store Update successfully.");

            await context.RespondAsync<BooleanResponseContract>(new
            {
                Status = true,
                Message = "Store Update successfully in corporate.",
                Timestamp = DateTime.Now
            });
        }
    }
}