using UniWeb.API.DataContext;
using UniWeb.API.DTO;

namespace UniWeb.API.DataServices
{
    public class UserDataServices:IUserDataService
    {
        private readonly EFDataContext _dataContext;

        public UserDataServices(EFDataContext dataContext) {
            _dataContext = dataContext;
        }


        public List<UserRegisterDto> GetUsers()
        {
            var users= _dataContext.Users.Select(x=> new UserRegisterDto()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate

            }).ToList();

            return users;
        }
    }
}
