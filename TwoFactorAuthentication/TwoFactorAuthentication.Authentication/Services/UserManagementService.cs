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
                                   , ITokenEncryptor tokenEncryptor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenEncryptor = tokenEncryptor;
        }
        public async Task<SignUpStatus> SignUp(CreateUserDto userDto)
        {
            //Check Logged in user token from database
            if (await _userRepository.CheckUserTokenExistence(userDto.Token) != 1)
            {
                return SignUpStatus.InvalidToken;
            }

            //Create hashed password and salt
            var hashedPassword = _passwordHasher.HashPasword(userDto.Password, out var salt);


            //New User ID
            var userId = Guid.NewGuid();
            //Create Token
            var token = _tokenEncryptor.Encrypt(userId.ToString());


            //TODO: Map Dto to Domain Entity and create new user
            var userEntity = new User_Login_Info()
            {
                Id = userId,
                Username = userDto.Username,
                Password = hashedPassword,
                Salt = salt,
                Token = token,

            };

            if (await _userRepository.CreateAsync(userEntity, nameof(StoredProcedureNames.users_login_info_CreateUser)) == 1)
            {
                return SignUpStatus.SignedUpSuccessfully;
            }

            return SignUpStatus.SignedUpFailed;
        }
    }
}
