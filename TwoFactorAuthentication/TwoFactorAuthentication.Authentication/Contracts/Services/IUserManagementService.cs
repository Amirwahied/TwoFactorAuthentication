using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;
using TwoFactorAuthentication.Authentication.Models;

namespace TwoFactorAuthentication.Authentication.Contracts.Services
{
    public interface IUserManagementService
    {
        Task<SignUpStatus> SignUp(CreateUserDto userDto, Guid createdByUserId);
        User_Login_Info CreateUserEntity(CreateUserDto userDto, Guid createdByUserId);
        Task<string> Enable2FA(Guid createdByUserId);
        Task<int> UpdateToken(Guid id, string token);
        Task<int> UpdateAuthenticatorKey(Guid id, string token);
    }
}
