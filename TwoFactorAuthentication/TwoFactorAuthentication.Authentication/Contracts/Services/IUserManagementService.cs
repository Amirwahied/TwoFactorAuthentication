using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;
using TwoFactorAuthentication.Authentication.Models;

namespace TwoFactorAuthentication.Authentication.Contracts.Services
{
	public interface IUserManagementService
	{
		Task<SignUpStatus> SignUp(CreateUserDto userDto, Guid createdByUserId);
		User_Login_Info CreateUserEntity(CreateUserDto userDto, Guid createdByUserId);
		Task Enable2FA(Guid createdByUserId);
		Task UpdateAuthenticatorKey(Guid id, string token);
	}
}
