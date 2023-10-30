
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Store;
using Greta.BO.Api.MassTransit;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Stores
{
    public class DeleteRangeStoreConsumer : BaseConsumer<DeleteRangeStoreRequestContract>
    {
        protected readonly IStoreRepository _storeRepository;

        public DeleteRangeStoreConsumer(
            ILogger<DeleteRangeStoreConsumer> logger,
            IStoreRepository storeRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _storeRepository = storeRepository;

        }

        public override async Task Execute(ConsumeContext<DeleteRangeStoreRequestContract> context)
        {
            this._logger.LogInformation("Start DeleteStore Consumer");

            //Delete Store             
            var store = await _storeRepository.GetEntity<Store>().Where(e => context.Message.StoresguidId.Contains(e.GuidId)).ToListAsync();
            await _storeRepository.DeleteRangeAsync(store);

            this._logger.LogInformation("Stores Delete successfully");

            await context.RespondAsync<BooleanResponseContract>(new
            {
                Status = true,
                Message = "Stores Delete successfully in corporate.",
                Timestamp = DateTime.Now
            });
        }
    }
}