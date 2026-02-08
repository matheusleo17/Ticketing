using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Tests.Fakes
{
    public class InMemoryTicketRepository : ITicketRepository
    {

        private readonly Dictionary<Guid, Ticket> _db = new();
        public Task Update(Ticket ticket)
        {
            _db[ticket.Id] = ticket;
            return Task.CompletedTask;
        }

        public Task<Ticket?> GetTicketById(Guid id)
        {
     
            if(_db.TryGetValue(id, out var ticket))
            {
                return Task.FromResult<Ticket?>(ticket);
            }
            return Task.FromResult<Ticket?>(null);
        }

         
    }
}
