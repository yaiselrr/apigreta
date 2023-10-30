using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Ticket;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Options;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.Sdk.MassTransit.Contracts;

namespace Greta.BO.Api.MassTransit.Ticket
{
    public class CancelTicketCreateStoreConsumer : BaseConsumer<CancelRangeTicketsRequestContract>
    {
        private readonly ITicketRepository _ticketRepository;

        public CancelTicketCreateStoreConsumer(
            ILogger<CancelTicketCreateStoreConsumer> logger,
            ITicketRepository ticketRepository,
            IOptions<MainOption> options
        )
            : base(logger, options)
        {
            _ticketRepository = ticketRepository;
        }

        public override async Task Execute(ConsumeContext<CancelRangeTicketsRequestContract> context)
        {
            //get tickets
            List<Entities.Ticket> tickets = new List<Entities.Ticket>();

            foreach (var guid in context.Message.TicketGuidIds)
            {
                var ticket = await _ticketRepository.GetEntity<Entities.Ticket>()
                    .FirstOrDefaultAsync(e => e.GuidId == guid);
                //cancel ticket
                ticket.Status = TicketStatus.CANCEL;
                tickets.Add(ticket);
            }

            var result = await this._ticketRepository.UpdateRangeAsync<Entities.Ticket>(tickets);

            if (result)
            {
                _logger.LogInformation("Tickets cancelation successfully");

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = true,
                    Message = "Tickets cancelation successfully",
                    Timestamp = DateTime.Now
                });
            }
            else
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string>() {"Error cancel ticket."},
                    Timestamp = DateTime.Now,
                });
        }
    }
}