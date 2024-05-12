using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwoFactorAuthentication._2FA.Contracts;
using TwoFactorAuthentication.Authentication.Constants;
using TwoFactorAuthentication.Authentication.Contracts.Services;
using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;
using IAuthenticationService = TwoFactorAuthentication.Authentication.Contracts.Services.IAuthenticationService;

namespace TwoFactorAuthentication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserManagementService userManagementService
                            , IAuthenticationService authenticationService
                            , ITwoFactorAuthService twoFactorAuthService
                            , IHttpContextAccessor httpContextAccessor)
        {
            _userManagementService = userManagementService;
            _authenticationService = authenticationService;
            _twoFactorAuthService = twoFactorAuthService;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize]
        public IActionResult SignUp()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SignUp(CreateUserDto userDto)
        {
            string? errorMessage = "";

            if (ModelState.IsValid)
            {

                var signupResult = await _userManagementService.SignUp(userDto, new Guid(((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.UserID)?.Value));


                switch (signupResult)
                {
                    case SignUpStatus.SignedUpSuccessfully:
                        //Logout current user
                        await _authenticationService.Logout();

                        //Redirect to login
                        return RedirectToAction(nameof(Login), "User");
                    case SignUpStatus.InvalidToken:
                        errorMessage = "Your token is invalid!";
                        break;
                    case SignUpStatus.SignedUpFailed:
                        errorMessage = "Createing new user failed!";
                        break;
                    case SignUpStatus.UsernameAlreadyUsed:
                        errorMessage = "Username already used!";
                        break;
                    case SignUpStatus.DatabaseError:
                        errorMessage = "Createing new user failed due to database error!";
                        break;
                    default:
                        break;
                }
            }
            TempData["ErrorMessage"] = errorMessage;
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            // If the user is already authenticated, redirect them to the home page
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDto userDto)
        {

            string? errorMessage = "";
            /* ModelState IsValid when
            * if user name not less than 4 char with no space
            * if password valid - at least(8 characters - one digit - one upper case - one lower case - one special character)
            */
            if (ModelState.IsValid)
            {
                // Log in user

                var loginResult = await _authenticationService.Login(userDto);

                switch (loginResult)
                {
                    case LoginStatus.LoggedInSuccessfully:
                        return RedirectToAction("Index", "Home");
                    case LoginStatus.TwoFactorRequired:
                        return RedirectToAction(nameof(TwoFactorAuthenticationCheck), "User");
                    case LoginStatus.InvalidUsernameOrPassword:
                        errorMessage = "Invalid username or password.";
                        break;
                    case LoginStatus.LoginFailed:
                        errorMessage = "Login failed.";
                        break;
                    case LoginStatus.DatabaseError:
                        errorMessage = "Login failed due to database error.";
                        break;
                    default:
                        break;
                }
            }

            // If ModelState is not valid or login was unsuccessful, set an error message
            TempData["ErrorMessage"] = errorMessage;
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await _authenticationService.Logout();

            // Redirect the user to the home page or a different page
            return RedirectToAction(nameof(Login), "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Enable2FA()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Enable2FA(bool isEnabled2FA)
        {
            if (!isEnabled2FA)
            {
                TempData["Message"] = await _userManagementService.Enable2FA(new Guid(((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.UserID)?.Value));
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthenticationConfiguration()
        {

            var qr = _twoFactorAuthService.GetQr(User.Identity.Name);

            _httpContextAccessor.HttpContext.Session.SetString("ImageUrl", qr.ImageUrl);
            _httpContextAccessor.HttpContext.Session.SetString("ManualKey", qr.ManualKey);

            await _userManagementService.UpdateAuthenticatorKey(new Guid(((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.UserID)?.Value), qr.ManualKey);

            // Redirect the user to the home page or a different page
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TwoFactorAuthenticationConfiguration(string pincode)
        {

            if (_twoFactorAuthService.VerifyAuthentication(pincode, User.Identity.Name))
            {
                // Redirect the user to the home page or a different page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ValidationResult"] = "Failed";
                return View();
            }

        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthenticationCheck()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TwoFactorAuthenticationCheck(string pincode)
        {

            if (_twoFactorAuthService.VerifyAuthentication(pincode, User.Identity.Name))
            {
                // Redirect the user to the home page or a different page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ValidationResult"] = "Failed";
                return View();
            }

        }




    }
}
