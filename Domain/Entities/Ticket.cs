using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public TicketStatus Status { get; set; }

        public bool Reserve() 
        { 
            if(this.Status == TicketStatus.Available)
            {
                Status = TicketStatus.Reserved;
                return true;
            }
            return false;
        }
    }
}
