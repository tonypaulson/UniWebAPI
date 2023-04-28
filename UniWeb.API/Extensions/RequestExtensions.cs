using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public static class RequestExtensions
    {
        private const string ErrorMessage = "Unable to parse token from header";
        public static int GetTenantId(this HttpRequest request)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var authHeader = request.Headers["Authorization"];

                if (!authHeader.Any())
                {
                    throw new UnauthorizedAccessException(ErrorMessage);
                }

                var authorization = authHeader[0];
                if (authorization.StartsWith("Bearer"))
                {
                    authHeader = authorization.Replace("Bearer ", "");
                    var jsonToken = handler.ReadToken(authHeader);
                    var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                    var tenantIdString = tokenS.Claims.First(claim => claim.Type == "TenantId").Value;
                    if (!string.IsNullOrEmpty(tenantIdString))
                    {
                        return Convert.ToInt32(tenantIdString);
                    }
                    throw new UnauthorizedAccessException(ErrorMessage);
                }
                else if (authorization.StartsWith("Basic"))
                {
                    var credentialsEncoded = authorization.Replace("Basic ", "");
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentialsEncoded));
                    var details = credentials.Split(":");
                    if ((details[0] == "anonymous") && (details[1] == "anonymous"))
                    {
                        if (!request.Headers.ContainsKey("TenantId")) 
                        {
                            throw new UnauthorizedAccessException("Tenant Id not specificied.");
                        }
                        var tenantIdHeader = request.Headers["TenantId"][0];
                        int tenantId = 0;
                        if (Int32.TryParse(tenantIdHeader, out tenantId)) 
                        {
                            return tenantId;
                        }
                    }
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch
            {
                throw new UnauthorizedAccessException(ErrorMessage);
            }
        }

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
