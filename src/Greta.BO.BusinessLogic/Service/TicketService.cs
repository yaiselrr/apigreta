using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface ITicketService : IGenericBaseService<Ticket>
    {
        Task<List<Ticket>> GetByStatus(TicketStatus ticketStatus);
        Task<bool> CancelRange(List<long> ids);
    }

    public class TicketService : BaseService<ITicketRepository, Ticket>, ITicketService
    {
        public TicketService(ITicketRepository repository, ILogger<TicketService> logger)
            : base(repository, logger)
        {
        }

        protected override IQueryable<Ticket> FilterqueryBuilder(
            Ticket filter,
            string searchstring,
            string[] splited,
            DbSet<Ticket> query)
        {
            IQueryable<Ticket> query1 = null;

            //if (!string.IsNullOrEmpty(searchstring))
            //{
            //    query1 = query.Where(c => c.Name.Contains(searchstring));
            //}
            //else
            //{
            //    query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));
            //}

            query1 = query1
                .Switch(splited)
                //.OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                //.OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e.Id);

            return query1;
        }

        public async Task<bool> CancelRange(List<long> ids)
        {
            List<Ticket> tickets = new List<Ticket>();
            foreach (var id in ids)
                tickets.Add(await _repository.GetEntity<Ticket>().FirstOrDefaultAsync(e => e.Id == id));
            foreach (var ticket in tickets)
                ticket.Status = TicketStatus.CANCEL;
            return await _repository.UpdateRangeAsync(tickets);
        }

        public async Task<List<Ticket>> GetByStatus(TicketStatus ticketStatus)
        {
            return await this._repository.GetEntity<Ticket>().Where(x => x.Status == ticketStatus).ToListAsync();
        }
    }
}