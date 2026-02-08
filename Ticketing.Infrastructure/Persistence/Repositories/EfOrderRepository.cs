using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Persistence;

public sealed class EFOrderRepository : IOrderRepository
{
    private readonly TicketingDbContext _context;

    public EFOrderRepository(TicketingDbContext context)
    {
        _context = context;
    }

    public async Task Add(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
}
