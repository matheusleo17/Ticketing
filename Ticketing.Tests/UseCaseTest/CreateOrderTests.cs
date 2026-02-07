using Ticketing.Application.Events;
using Ticketing.Application.UseCases;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;
using Ticketing.Tests.Clock;
using Ticketing.Tests.Fakes;

namespace Ticketing.Tests.UseCaseTest
{
    public class CreateOrderTests
    {
        [Fact]
        public async Task Should_create_order_and_publish_event_when_ticket_is_available()
        {
            var ticketId = Guid.NewGuid();
            var buyerId = Guid.NewGuid();

            var ticket = new Ticket 
            { 
                Id = ticketId, 
                Status = TicketStatus.Available 
            };
            var ticketRepository = new InMemoryTicketRepository();
            await ticketRepository.SaveTicket(ticket);

            var orderRepository = new InMemoryOrderRepository();
            var eventBus = new FakeEventBus();

            var now = new DateTime(2026, 1, 1, 10, 0, 0);
            var clock = new FakeClock(now);

            var useCase = new CreateOrderUseCase(
                    ticketRepository,
                    clock,
                    orderRepository,
                    eventBus);

            var order = await useCase.CreateOrder(ticketId, buyerId);

            Assert.NotNull(order);
            Assert.Equal(TicketStatus.Reserved, ticket.Status);
            Assert.Single(eventBus.PublishedEvents);
            Assert.IsType<OrderCreated>(eventBus.PublishedEvents[0]);

        }
        [Fact]
        public async Task Should_not_create_order_or_publish_event_when_ticket_is_reserved()
        {
            var ticketId = Guid.NewGuid();
            var buyerId = Guid.NewGuid();

            var ticket = new Ticket
            {
                Id = ticketId,
                Status = TicketStatus.Reserved
            };
            var ticketRepository = new InMemoryTicketRepository();
            await ticketRepository.SaveTicket(ticket);

            var orderRepository = new InMemoryOrderRepository();
            var eventBus = new FakeEventBus();

            var now = new DateTime(2026, 1, 1, 10, 0, 0);
            var clock = new FakeClock(now);

            var useCase = new CreateOrderUseCase(
                    ticketRepository,
                    clock,
                    orderRepository,
                    eventBus);
            var order = await useCase.CreateOrder(ticketId, buyerId);

            Assert.Null(order);
            Assert.Empty(eventBus.PublishedEvents);
            Assert.Equal(TicketStatus.Reserved, ticket.Status);

        }

    }
}
