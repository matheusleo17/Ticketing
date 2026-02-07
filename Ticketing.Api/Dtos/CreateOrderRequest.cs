namespace Ticketing.Api.Dtos
{
    public class CreateOrderRequest
    {
        public Guid TicketId { get; set; }
        public Guid BuyerId { get; set; }
    }
}
