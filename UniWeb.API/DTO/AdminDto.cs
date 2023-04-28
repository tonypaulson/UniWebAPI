using UniWeb.API.Entities;

namespace UniWeb.API.DTO
{
    public class AdminDto 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public static Admin FromAdmin(Admin admin)
        {
            var dto = new Admin
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                MobileNumber = admin.MobileNumber,
                Password = admin.Password,
                CreatedDate = admin.CreatedDate,
                UpdatedDate = admin.UpdatedDate

            };
            return dto;
        }
    }
}