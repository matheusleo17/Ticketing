using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task Update(Ticket ticket);
        Task<Ticket?> GetTicketById(Guid id);
    }
}
