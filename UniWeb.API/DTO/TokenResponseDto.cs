using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.DTO.AccountDto
{
    public class TokenResponseDto
    {
        public string Token { get; set; }   // jwt token
        public DateTime Expiration { get; set; }
        public string Refresh_Token { get; set; }
        public string Username { get; set; }
        public string CurrencyUnit { get; set; }
        public int userId { get; set; }
    }
}
