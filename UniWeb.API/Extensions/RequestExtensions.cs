using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public static class RequestExtensions
    {
        private const string ErrorMessage = "Unable to parse token from header";
       

        public static int GetUserId(this HttpRequest request)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var authHeader = request.Headers["Authorization"];

                if (!authHeader.Any())
                {
                    throw new UnauthorizedAccessException(ErrorMessage);
                }

                authHeader = authHeader[0].Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                var userIdString = tokenS.Claims.First(claim => claim.Type == "UserId").Value;
                if (!string.IsNullOrEmpty(userIdString))
                {
                    return Convert.ToInt32(userIdString);
                }

                throw new UnauthorizedAccessException(ErrorMessage);
            }
            catch
            {
                throw new UnauthorizedAccessException(ErrorMessage);
            }
        }

        public static TimeZoneInfo GetTimeZoneInfo(this HttpRequest request)
        {
            var timezoneName = "";
            if (request.Headers.ContainsKey("timezone"))
            {
                timezoneName = request.Headers["timezone"];
            }
            else if(request.Headers.ContainsKey("TimeZone"))
            {
                timezoneName = request.Headers["TimeZone"];
            }
            else {
                return null;
            }

            var timeZoneInfo = TimeZoneInfo.GetSystemTimeZones()
            .Where(x => x.StandardName == timezoneName)
            .FirstOrDefault();

            return timeZoneInfo;
        }

        public static string GetTimeZone(this HttpRequest request)
        {
            try
            {
                var timeZone = request.Headers["timezone"];
                if (!string.IsNullOrEmpty(timeZone))
                {
                    return timeZone;
                }

                throw new UnauthorizedAccessException(ErrorMessage);
            }
            catch
            {
                throw new UnauthorizedAccessException(ErrorMessage);
            }
        }

        public static DateTime GetBrowserTime(this HttpRequest request)
        {
            try
            {
                var dateTimeNow = request.Headers["DateTimeNow"];
                if (!string.IsNullOrEmpty(dateTimeNow))
                {
                    return Convert.ToDateTime(dateTimeNow);
                }

                throw new UnauthorizedAccessException(ErrorMessage);
            }
            catch
            {
                throw new UnauthorizedAccessException(ErrorMessage);
            }
        }
    }
}
