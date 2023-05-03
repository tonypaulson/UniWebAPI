using UniWeb.API.DTO.AccountDto;
using UniWeb.API.DTO;
using UniWeb.API.Entities;

namespace UniWeb.API.Services
{
    public interface IAdminService
    {

        Admin Create(Admin admin);

        Task<TokenResponseDto> GenerateNewTokenadmin(TokenRequestDto tokenRequestDto);
        Task<TokenResponseDto> RefreshToken(TokenRequestDto tokenRequestDto);

        Admin Userlogin(string email, string password);

        TokenResponseDto CreateAccessTokenAdmin(Admin admin, int adminId, AdminToken newRefreshtoken);
        bool ResetPassword(PasswordResetDto dto);
        bool IsPasswordEqual(string password, int userId);
    }
}
