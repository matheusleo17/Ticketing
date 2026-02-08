using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Persistence.Repositories
{
    public class EfTicketRepository : ITicketRepository
    {
        private readonly TicketingDbContext _context;

        public EfTicketRepository(TicketingDbContext ticketDbContext) 
        { 
            _context = ticketDbContext;
        }

        public Task<Ticket?> GetTicketById(Guid id)
        {
            var existsId = _context.Tickets.FirstOrDefaultAsync(_ => _.Id == id);

            return existsId;
        }

        public Task SaveTicket(Ticket ticket)
        {
          return _context.SaveChangesAsync();

        }
    }
}
