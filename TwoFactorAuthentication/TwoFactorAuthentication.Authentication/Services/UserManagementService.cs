using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using TwoFactorAuthentication.Authentication.Constants;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManagementService(IUserRepository userRepository
                                   , IPasswordHasher passwordHasher
                                   , ITokenEncryptor tokenEncryptor
                                   , IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenEncryptor = tokenEncryptor;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<string> Enable2FA(Guid id)
        {
            try
            {
                //Check Logged in user token from database
                if (await _userRepository.Enable2FA(id) > 0)
                {
                    _httpContextAccessor.HttpContext.Session.SetString(CustomClaimsTypes.IsTwoFactorEnabled, "True");
                    return "2FA Enabled Successfully";
                }
                return "2FA Not Enabled";
            }
            catch (SqlException ex)
            {
                return $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        public async Task<SignUpStatus> SignUp(CreateUserDto userDto, Guid createdByUserId)
        {
            try
            {
                //Check Logged in user token from database
                if (await _userRepository.CheckUserTokenExistence(userDto.Token) != 1)
                {
                    return SignUpStatus.InvalidToken;
                }

                await _userRepository.CreateAsync(CreateUserEntity(userDto, createdByUserId), nameof(StoredProcedureNames.users_login_info_CreateUser));
                return SignUpStatus.SignedUpSuccessfully;
            }
            catch (SqlException ex)
            {
                //Username duplicated
                if (ex.Message.Contains("Cannot insert duplicate key"))
                {
                    return SignUpStatus.UsernameAlreadyUsed;
                }
                return SignUpStatus.DatabaseError;
            }
            catch (Exception)
            {
                return SignUpStatus.SignedUpFailed;
            }
        }

        public async Task<int> UpdateToken(Guid id, string token)
        {
            //try
            //{
            return await _userRepository.UpdateToken(id, token);
            //}
            //catch (SqlException ex)
            //{
            //	return $"SQL Error: {ex.Message}";
            //}
            //catch (Exception ex)
            //{
            //	return $"Exception: {ex.Message}";
            //}
        }

        public async Task<int> UpdateAuthenticatorKey(Guid id, string token)
        {
            //try
            //{
            return await _userRepository.UpdateAuthenticatorKey(id, token);
            //}
            //catch (SqlException ex)
            //{
            //	return $"SQL Error: {ex.Message}";
            //}
            //catch (Exception ex)
            //{
            //	return $"Exception: {ex.Message}";
            //}
        }
    }
}
