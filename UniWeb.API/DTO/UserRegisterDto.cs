using UniWeb.API.Entities;

namespace UniWeb.API.DTO
{
    public class UserRegisterDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public static UserRegisterDto FromUser(User user)
        {
            var dto = new UserRegisterDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Gender = user.Gender,
                Password = user.Password,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate

            };
            return dto;
        }
    }
}