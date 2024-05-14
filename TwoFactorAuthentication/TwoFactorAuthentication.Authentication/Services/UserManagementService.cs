using TwoFactorAuthentication.Authentication.Contracts.Repositories;
using TwoFactorAuthentication.Authentication.Contracts.Services;
using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;
using TwoFactorAuthentication.Authentication.Models;
using TwoFactorAuthentication.Core.Contracts;
using TwoFactorAuthentication.Core.Enums;

namespace TwoFactorAuthentication.Authentication.Services
{
    internal class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenEncryptor _tokenEncryptor;

        public UserManagementService(IUserRepository userRepository
                                   , IPasswordHasher passwordHasher
                                   , ITokenEncryptor tokenEncryptor
                                    )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenEncryptor = tokenEncryptor;
        }

        public User_Login_Info CreateUserEntity(CreateUserDto userDto, Guid createdByUserId)
        {
            //Create hashed password and salt
            var hashedPassword = _passwordHasher.HashPasword(userDto.Password, out var salt);


            //New User ID
            var userId = Guid.NewGuid();
            //Create Token
            var token = _tokenEncryptor.Encrypt(userId.ToString());


            // Map Dto to Domain Entity and create new user
            return new User_Login_Info()
            {
                Id = userId,
                Username = userDto.Username,
                Password = hashedPassword,
                Salt = salt,
                Token = token,
                Created_By = createdByUserId
            };
        }

        public async Task Enable2FA(Guid id)
        {
            await _userRepository.Enable2FA(id);
        }

        public async Task<SignUpStatus> SignUp(CreateUserDto userDto, Guid createdByUserId)
        {

            if (await _userRepository.CheckUserTokenExistence(userDto.Token) != 1)
            {
                return SignUpStatus.InvalidToken;
            }

            await _userRepository.CreateAsync(CreateUserEntity(userDto, createdByUserId), nameof(StoredProcedureNames.users_login_info_CreateUser));
            return SignUpStatus.SignedUpSuccessfully;
        }

        public async Task UpdateAuthenticatorKey(Guid id, string token)
        {
            await _userRepository.UpdateAuthenticatorKey(id, token);
        }
    }
}
