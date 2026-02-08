using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task Add(Order order);
    }
}
