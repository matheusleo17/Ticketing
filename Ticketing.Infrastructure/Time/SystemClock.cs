using Ticketing.Application.Interfaces;

namespace Ticketing.Infrastructure.Time
{
    public class SystemClock : IClock
    {
        private readonly DateTime date;
  
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
