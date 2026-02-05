using System.Net.NetworkInformation;
using Ticketing.Application.Events;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.UseCases
{
    public class CreateOrderUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IClock _clock;
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _eventBus;

        public CreateOrderUseCase(
            ITicketRepository ticketRepository,
            IClock clock,
            IOrderRepository order,
            IEventBus eventBus)
        {
            _clock = clock;
            _ticketRepository = ticketRepository;
            _orderRepository = order;
            _eventBus = eventBus;
        }

        public async Task<Order?> AddOrder(Guid ticketId, Guid buyerId)
        {
            var ticket = await _ticketRepository.GetTicketById(ticketId);

            if(ticket is not null)
            {
                var reserved = ticket.Reserve();

                if (reserved)
                { 
                    var newOder = new Order
                    {
                        Id = Guid.NewGuid(),
                        TicketId = ticketId,
                        BuyerId = buyerId,
                        Status = OrderStatus.Created,
                        ExpiresAt = _clock.Now().AddMinutes(1)
                    };
                    await _ticketRepository.SaveTicket(ticket);
                    await _orderRepository.SaveOrder(newOder);

                    await _eventBus.Publish(new OrderCreated(newOder.Id, newOder.TicketId));

                    return newOder;
                  
                }

            }
            return null;
        }

        
    }
}
