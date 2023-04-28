using System;

namespace UniWeb.API.Entities
{
    public class TemporaryPassword
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
    }
}