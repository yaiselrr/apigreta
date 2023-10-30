using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Ticket;
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
    public class CancelTicketConsumer : BaseConsumer<CancelTicketRequestContract>
    {
        private readonly ITicketRepository _ticketRepository;

        public CancelTicketConsumer(
            ILogger<CancelTicketConsumer> logger,
            ITicketRepository ticketRepository,
            IOptions<MainOption> options
        )
            : base(logger, options)
        {
            _ticketRepository = ticketRepository;
        }

        public override async Task Execute(ConsumeContext<CancelTicketRequestContract> context)
        {
            //get ticket
            var ticket = await _ticketRepository.GetEntity<Entities.Ticket>()
                .FirstOrDefaultAsync(e => e.GuidId == context.Message.GuidId);

            //cancel ticket
            ticket.Status = TicketStatus.CANCEL;
            var result = await this._ticketRepository.UpdateAsync(ticket);

            if (result)
            {
                _logger.LogInformation("Ticket cancelation successfully");

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = true,
                    Message = "Ticket cancelation successfully",
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