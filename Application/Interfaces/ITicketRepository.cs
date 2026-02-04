using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task SaveTicket(Ticket ticket);
        Task<Ticket?> GetTicketById(Guid id);
    }
}
