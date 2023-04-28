using System;
using System.Collections.Generic;

namespace UniWeb.API.Entities
{
    public class Admin
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IEnumerable<AdminToken> Tokens { get; set; }
    }
}
