using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public int TenantId { get; set; }
    }
}
