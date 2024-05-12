using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;

namespace TwoFactorAuthentication.Authentication.Contracts.Services
{
    public interface IAuthenticationService
    {
        Task<LoginStatus> Login(LoginUserDto loginUserDto);
        Task Logout();

    }
}
