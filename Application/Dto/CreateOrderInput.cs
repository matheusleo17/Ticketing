namespace Ticketing.Application.Dto
{
    public class CreateOrderInput
    {
        public Guid TicketId { get; set; }
        public Guid BuyerId { get; set; }
        public int Amount { get; set; }
    }
}
