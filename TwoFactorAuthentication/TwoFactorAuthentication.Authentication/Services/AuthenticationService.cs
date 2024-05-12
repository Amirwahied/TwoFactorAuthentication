using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TwoFactorAuthentication.Authentication.Constants;
using TwoFactorAuthentication.Authentication.Contracts.Repositories;
using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;
using TwoFactorAuthentication.Core.Contracts;
using IAuthenticationService = TwoFactorAuthentication.Authentication.Contracts.Services.IAuthenticationService;

namespace TwoFactorAuthentication.Authentication.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor
                                   , IUserRepository userRepository
                                   , IPasswordHasher passwordHasher)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginStatus> Login(LoginUserDto loginUserDto)
        {
            //get user from DB by username
            var storedUser = await _userRepository.GetByUsername(loginUserDto.Username);

            //if not found -> not valid username or password
            if (storedUser is null)
            {
                return LoginStatus.InvalidUsernameOrPassword;
            }


            //compare hashed password
            var result = _passwordHasher.VerifyPassword(loginUserDto.Password, storedUser.Password, storedUser.Salt);

            //if not valid (invalid password)
            if (!result)
            {
                return LoginStatus.InvalidUsernameOrPassword;
            }

            //if valid 

            /* 
             * 
             * open welcome page
             */

            //set user active if not
            if (!storedUser!.Is_Active)
            {
                await _userRepository.ActivateUser(storedUser.Id);
            }

            //Create User Identity
            var identity = new ClaimsIdentity(new[] { new Claim(CustomClaimsTypes.Username, storedUser.Username)
                                                        , new Claim(CustomClaimsTypes.UserID, storedUser.Id.ToString())
                                                        , new Claim(CustomClaimsTypes.Token,storedUser.Token)
                                                        }
                                                    , CookieAuthenticationDefaults.AuthenticationScheme);

            //await _httpContextAccessor.HttpContext.SignOutAsync();


            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.MaxValue,
                    AllowRefresh = true
                });

            return LoginStatus.LoggedInSuccessfully;
        }

        public async Task Logout()
        {
            // Sign out the user
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}
