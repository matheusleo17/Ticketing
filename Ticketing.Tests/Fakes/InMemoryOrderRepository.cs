using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace Ticketing.Tests.Fakes
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly Dictionary<Guid, Order> _db = new();
        public Task SaveOrder(Order order)
        {
            _db[order.Id] = order;
            return Task.CompletedTask;
        }
    }
}
