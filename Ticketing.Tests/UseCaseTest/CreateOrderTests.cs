using System.Net.Sockets;
using Ticketing.Application.Common;
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
            await ticketRepository.Update(ticket);

            var orderRepository = new InMemoryOrderRepository();
            var eventBus = new FakeEventBus();

            var now = new DateTime(2026, 1, 1, 10, 0, 0);
            var clock = new FakeClock(now);

            var useCase = new CreateOrderUseCase(
                    ticketRepository,
                    clock,
                    orderRepository,
                    eventBus);

            var result = await useCase.CreateOrder(ticketId, buyerId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

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
            await ticketRepository.Update(ticket);

            var orderRepository = new InMemoryOrderRepository();
            var eventBus = new FakeEventBus();

            var now = new DateTime(2026, 1, 1, 10, 0, 0);
            var clock = new FakeClock(now);

            var useCase = new CreateOrderUseCase(
                    ticketRepository,
                    clock,
                    orderRepository,
                    eventBus);
            var result = await useCase.CreateOrder(ticketId, buyerId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.TicketAlreadyReserved, result.Error);

            Assert.Empty(eventBus.PublishedEvents);
            Assert.Equal(TicketStatus.Reserved, ticket.Status);

        }
        [Fact]
        public async Task TaskShould_return_error_when_ticket_does_not_exist()
        {
            var ticketId = Guid.NewGuid();
            var buyerId = Guid.NewGuid();

            var ticketRepository = new InMemoryTicketRepository();
            var orderRespository = new InMemoryOrderRepository();
            var eventBus = new FakeEventBus();
            var now = new DateTime(2026, 1, 1, 10, 0, 0);
            var clock = new FakeClock(now);

            var useCase = new CreateOrderUseCase(ticketRepository, clock, orderRespository, eventBus);

            var result = await useCase.CreateOrder(ticketId, buyerId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.TicketNotFound, result.Error);
            Assert.Empty(eventBus.PublishedEvents);


        }
        [Fact]
        public async Task Should_publish_event_only_once_when_creating_order_twice_for_same_ticket()
        {
            var ticketId = Guid.NewGuid();
            var buyerId = Guid.NewGuid();

            var ticket = new Ticket
            {
                Id = ticketId,
                Status = TicketStatus.Available
            };

            var ticketRepository = new InMemoryTicketRepository();
            var orderRespository = new InMemoryOrderRepository();
            var eventBus = new FakeEventBus();
            var now = new DateTime(2026, 1, 1, 10, 0, 0);
            var clock = new FakeClock(now);

            var useCase = new CreateOrderUseCase(ticketRepository, clock, orderRespository, eventBus);

            var firstResult = await useCase.CreateOrder(ticketId, buyerId);
            var secondResult = await useCase.CreateOrder(ticketId, buyerId);

            Assert.True(firstResult.IsSuccess);
            Assert.False(secondResult.IsSuccess);
            Assert.Equal(ErrorType.TicketAlreadyReserved, secondResult.Error);

            Assert.Single(eventBus.PublishedEvents);
            Assert.Equal(TicketStatus.Reserved, ticket.Status);

        }
        [Fact]
        public async Task Should_allow_only_one_order_when_creating_concurrently()
        {
            var ticketId = Guid.NewGuid();
            var buyerId = Guid.NewGuid();

            var ticket = new Ticket
            {
                Id = ticketId,
                Status = TicketStatus.Available
            };

            var ticketRepository = new InMemoryTicketRepository();
            var orderRespository = new InMemoryOrderRepository();
            var eventBus = new FakeEventBus();
            var now = new DateTime(2026, 1, 1, 10, 0, 0);
            var clock = new FakeClock(now);

            var useCase = new CreateOrderUseCase(ticketRepository, clock, orderRespository, eventBus);

            var tasks = new[]
            {
                Task.Run(()=>useCase.CreateOrder(ticketId, buyerId)),
                Task.Run (() => useCase.CreateOrder(ticketId, buyerId))
            };
            var results = await Task.WhenAll(tasks);
            var successCount = results.Count(r => r.IsSuccess);
            var failureCount = results.Count(r => !r.IsSuccess);

            Assert.Equal(1, successCount);
            Assert.Equal(1, failureCount);

            Assert.Single(eventBus.PublishedEvents);
            Assert.Equal(TicketStatus.Reserved, ticket.Status);



        }
    }
}