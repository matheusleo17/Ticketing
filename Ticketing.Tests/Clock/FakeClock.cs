using Ticketing.Application.Interfaces;

namespace Ticketing.Tests.Clock
{
    public class FakeClock : IClock
    {
        private readonly DateTime date;
        public FakeClock(DateTime dateNow) 
        {
            date = dateNow;
        }

        public DateTime Now()
        {
            return date;
        }
    }
}
