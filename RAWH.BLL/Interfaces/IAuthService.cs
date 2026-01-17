using RAWH.BLL.DTOs;

namespace RAWH.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginDTO dto);
        Task<string> Register(RegisterDTO dto);
        Task<AuthModelDTO> SendResetCode(string email);
        Task<AuthModelDTO> VerifyResetCode(string code);
        Task<AuthModelDTO> SetNewPassword(NewPasswordDTO model);
        Task<string> GoogleLogin(string idToken);

    }
}
