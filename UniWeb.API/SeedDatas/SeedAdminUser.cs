using UniWeb.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Carewell.API.SeedDatas
{
    public class SeedAdminUser
    {
        public User[] Seed()
        {
            return new User[]
            {
                new User{Id=1, FirstName="user", LastName="user", Email="user@uniweb.com", Password=HashPassword("user"),MobileNumber="9995222222",Gender=1 }
            };
        }

        private string HashPassword(string password)
        {
            var hasher = SHA256.Create();
            var hash = hasher.ComputeHash(Encoding.Default.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}
