using System.Security.Cryptography;
using System.Text;
using UniWeb.API.Entities;

namespace Carewell.API.SeedDatas
{
    public class SeedAdmin
    {


        public Admin[] Seed()
        {

            return new Admin[]
            {
                new Admin{Id =1,FirstName = "admin",LastName = "admin",Email = "admin@uniweb.com",MobileNumber="9282827262",Password=HashPassword("admin") }
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
