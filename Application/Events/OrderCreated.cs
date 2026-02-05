namespace Ticketing.Application.Events
{
   public record OrderCreated(Guid OrderId, Guid TicketId);
}
