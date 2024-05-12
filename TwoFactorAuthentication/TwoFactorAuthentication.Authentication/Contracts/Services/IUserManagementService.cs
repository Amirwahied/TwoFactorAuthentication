using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;

namespace TwoFactorAuthentication.Authentication.Contracts.Services
{
    public interface IUserManagementService
    {
        Task<SignUpStatus> SignUp(CreateUserDto userDto);
    }
}
