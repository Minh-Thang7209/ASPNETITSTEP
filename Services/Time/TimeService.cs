namespace ASPNETITSTEP.Services.Time
{
    public class TimeService : ITimeService
    {
        public long GetTimestamp()
        {
           return System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}