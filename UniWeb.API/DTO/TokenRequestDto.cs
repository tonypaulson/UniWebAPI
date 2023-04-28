using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.DTO
{
    public class TokenRequestDto
    {
        public string GrantType { get; set; }   // password or refresh_token
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }
    }
}
