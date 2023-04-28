using System;
using System.Security.Cryptography;
using System.Text;

namespace UniWeb.API.Services
{
    public class PasswordHaser
    {
        public string HashPassword(string password)
        {
            if (password != null)
            {
                var hasher = SHA256.Create();
                var hash = hasher.ComputeHash(Encoding.Default.GetBytes(password));
                return Convert.ToBase64String(hash);
            }

            return string.Empty;
        }
    }
}