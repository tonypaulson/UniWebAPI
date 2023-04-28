using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.DTO
{
    public class PasswordResetDto
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
