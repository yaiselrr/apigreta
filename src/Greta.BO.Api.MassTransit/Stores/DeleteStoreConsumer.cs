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
    public class DeleteStoreConsumer : BaseConsumer<DeleteStoreRequestContract>
    {
        private readonly IStoreRepository _storeRepository;

        public DeleteStoreConsumer(
            ILogger<DeleteStoreConsumer> logger,
            IStoreRepository storeRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _storeRepository = storeRepository;
        }

        public override async Task Execute(ConsumeContext<DeleteStoreRequestContract> context)
        {
            this._logger.LogInformation("Start DeleteStore Consumer");

            //Delete Store             
            var store = await _storeRepository.GetEntity<Store>()
                .FirstOrDefaultAsync(e => e.GuidId == context.Message.GuidId);
            await _storeRepository.DeleteAsync(store.Id);

            this._logger.LogInformation("Store Delete successfully");

            await context.RespondAsync<BooleanResponseContract>(new
            {
                Status = true,
                Message = "Store Delete successfully in corporate.",
                Timestamp = DateTime.Now
            });
        }
    }
}