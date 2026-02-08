using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Persistence;

public  class EFTicketRepository : ITicketRepository
{
    private readonly TicketingDbContext _context;

    public EFTicketRepository(TicketingDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket?> GetTicketById(Guid id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    public async Task Update(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }
}
