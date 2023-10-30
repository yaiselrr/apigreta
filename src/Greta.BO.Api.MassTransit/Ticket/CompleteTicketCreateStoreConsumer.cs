using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.EventContracts.Ticket;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Greta.BO.Api.MassTransit.Ticket
{
    public class CompleteTicketCreateStoreConsumer : BaseConsumer<CompleteTicketCreateStoreRequestContract>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IStoreRepository _storeRepository;
        readonly MainOption options;

        public CompleteTicketCreateStoreConsumer(
            ILogger<CompleteTicketCreateStoreConsumer> logger,
            ITicketRepository ticketRepository,
            IStoreRepository storeRepository,
            IOptions<MainOption> options
        )
            : base(logger, options)
        {
            this.options = options.Value;
            _ticketRepository = ticketRepository;
            _storeRepository = storeRepository;
        }

        public override async Task Execute(ConsumeContext<CompleteTicketCreateStoreRequestContract> context)
        {
            //get ticket
            var ticket = await _ticketRepository.GetEntity<Entities.Ticket>()
                .FirstOrDefaultAsync(e => e.GuidId == context.Message.GuidId);

            var result = false;
            switch (ticket.Type)
            {
                case TicketType.EMPTY:
                    break;
                case TicketType.CREATE_STORE:
                    //create store
                    result = await CreateStore(context, ticket);
                    break;
                case TicketType.ADD_DEVICE:
                    break;
                case TicketType.BREAK:
                    break;
                case TicketType.COMPLAIN:
                    break;
                case TicketType.DELETE_STORE:
                    result = await DeleteStore(context, ticket);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!result)
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string>() { "Error create store." },
                    Timestamp = DateTime.Now,
                });
        }

        private async Task<bool> DeleteStore(ConsumeContext context, Entities.Ticket ticket)
        {
            try
            {
                var storeIdS = ticket.Data.Split(",").Select(long.Parse).ToList();
                var boStores = await _storeRepository.GetEntity<Store>()
                    .Where(x => storeIdS.Contains(x.Id)).ToListAsync();

                //remove
                await _storeRepository.DeleteRangeAsync(boStores);

                //complete ticket
                ticket.Status = TicketStatus.COMPLETE;
                var result = await this._ticketRepository.UpdateAsync(ticket);

                if (!result) return false;
                await context.RespondAsync<CompleteTicketCreateStoreResponseContract>(new
                {
                    Data = ticket.Data,
                });
                _logger.LogInformation("Ticket complete successfully");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<bool> CreateStore(ConsumeContext context, Entities.Ticket ticket)
        {
            var store = JsonConvert.DeserializeObject<Store>(ticket.Data);
            store.UserCreatorId = userId;
            store.Region = null;
            var boStore = await _storeRepository.CreateAsync<Store>(store);
            //complete ticket
            ticket.Status = TicketStatus.COMPLETE;
            var result = await this._ticketRepository.UpdateAsync(ticket);

            if (!result) return false;
            await context.RespondAsync<CompleteTicketCreateStoreResponseContract>(new
            {
                Data = boStore.Id,
            });
            _logger.LogInformation("Ticket complete successfully");
            return true;
        }
    }
}