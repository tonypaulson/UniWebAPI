using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace UniWeb.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TokenDecryptor
    {
        private readonly RequestDelegate _next;

        private string GetToken(string authHeader)
        {
            //var handler = new JwtSecurityTokenHandler();
            //authHeader = authHeader.Replace("Bearer ", "");
            //var jsonToken = handler.ReadToken(authHeader);
            //var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            //var id = tokenS.Claims.First(claim => claim.Type == "TenantId").Value;

            return "1";
        }

        public TokenDecryptor(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //var principal = httpContext.User;
            //if (principal?.Claims != null)
            //{
            //    foreach (var claim in principal.Claims)
            //    {

            //    }

            //}

            //var token = httpContext.Request.Headers["Authorization"];
            //var tenantId = GetToken(token);
            //httpContext.Request.Headers.Add("TenantId", tenantId);
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TokenDecryptorExtensions
    {
        public static IApplicationBuilder UseTokenDecryptor(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenDecryptor>();
        }
    }
}
