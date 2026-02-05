namespace Ticketing.Application.Interfaces
{
    public interface IEventBus
    {
        Task Publish<T>(T @event);
    }
}
