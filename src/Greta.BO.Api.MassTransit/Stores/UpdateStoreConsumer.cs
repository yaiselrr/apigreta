using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Store;
using Greta.BO.BusinessLogic.Exceptions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Stores
{
    public class UpdateStoreConsumer : BaseConsumer<UpdateStoreRequestContract>
    {
        private readonly IStoreRepository _storeRepository;

        public UpdateStoreConsumer(
            ILogger<UpdateStoreConsumer> logger,
            IStoreRepository storeRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _storeRepository = storeRepository;
        }

        public override async Task Execute(ConsumeContext<UpdateStoreRequestContract> context)
        {
            this._logger.LogInformation("Start UpdateStore Consumer.");

            //Update Store 
            var store = await _storeRepository.GetEntity<Store>()
                .FirstOrDefaultAsync(e => e.GuidId == context.Message.GuidId);
            store.Name = context.Message.Name;

            await _storeRepository.UpdateAsync(store);

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