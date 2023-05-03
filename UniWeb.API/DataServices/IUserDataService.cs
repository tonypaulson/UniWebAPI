using UniWeb.API.DTO;

namespace UniWeb.API.DataServices
{
    public interface IUserDataService
    {

        List<UserRegisterDto> GetUsers();
    }
}
