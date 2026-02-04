using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid TicketId {  get; set; }
        public Guid BuyerId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime ExpiresAt { get; set; }

        public bool Reserve()
        {
            if(this.Status == OrderStatus.Created)
            {
                Status = OrderStatus.Reserved;
                return true;
            } 
            return false;

        }
        public bool Confirm(DateTime now)
        {
            if(this.Status == OrderStatus.Reserved)
            {
                if(ExpiresAt>= now)
                {
                    Status = OrderStatus.Confirmed;
                    return true;
                }
                else
                {
                    Status = OrderStatus.Expired;
                    return false;
                }
                
            }
            return false;
        }

    }
}
