using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Persistence.Repositories
{
    public  class EfOrderRepository : IOrderRepository
    {
        private readonly TicketingDbContext _context;

        public EfOrderRepository(TicketingDbContext ticketDbContext)
        {
            _context = ticketDbContext;
        }
        public Task SaveOrder(Order order)
        {
            return _context.SaveChangesAsync();
        }
    }
}
