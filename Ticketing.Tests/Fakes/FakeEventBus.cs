using Ticketing.Application.Interfaces;

namespace Ticketing.Tests.Fakes
{
    public class FakeEventBus : IEventBus
    {
        public List<Object> PublishedEvents { get; } = new();
        public Task Publish<T>(T @event)
        {
            PublishedEvents.Add(@event!);

            return Task.CompletedTask;

        }
    }
}
