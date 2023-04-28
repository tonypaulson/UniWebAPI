using System;

namespace UniWeb.API.Services
{
    public class TimeService
    {
        public DateTime GetDateFromTimestamp(long timestamp)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            date = date.AddSeconds(timestamp);
            date = date.ToLocalTime();
            return date;
        }

        public long GetTimestampFromDate(DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                date = date.ToUniversalTime();
            }
            var baseDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var offset = date.Subtract(baseDate);
            var totalSeconds = offset.TotalSeconds;
            return Convert.ToInt64(totalSeconds);
        }
    }
}