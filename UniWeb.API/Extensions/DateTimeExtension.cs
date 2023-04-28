using TimeZoneConverter;

namespace System
{
    public static class DateTimeExtension
    {
        public static DateTime ConvertUTCToLocalTimeBasedOnTimeZone(this DateTime request, string timeZone)
        {
            try
            {
                var utcDateTime = Convert.ToDateTime(request);
                TimeZoneInfo nzTimeZone = TZConvert.GetTimeZoneInfo(timeZone); //TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                DateTime nzDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, nzTimeZone);
                return nzDateTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime ConvertLocalTimeToUTCBasedOnTimeZone(this DateTime request, string timeZone)
        {
            try
            {
                var utcDateTime = Convert.ToDateTime(request);
                TimeZoneInfo nzTimeZone = TZConvert.GetTimeZoneInfo(timeZone); //TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                DateTime nzDateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(utcDateTime, DateTimeKind.Unspecified), nzTimeZone);
                return nzDateTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
