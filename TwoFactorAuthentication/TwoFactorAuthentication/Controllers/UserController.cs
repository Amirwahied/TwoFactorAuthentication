using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserManagementService userManagementService
                            , IAuthenticationService authenticationService
                            , IHttpContextAccessor httpContextAccessor)
        {
            _userManagementService = userManagementService;
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(CreateUserDto userDto)
        {
            if (ModelState.IsValid)
            {

                if (await _userManagementService.SignUp(userDto) > 0)
                {
                    TempData["SuccessMessage"] = "User created successfully!";

                    //Logout current user
                    await _authenticationService.Logout();

                    //Redirect to login
                    return RedirectToAction(nameof(Login), "User");
                }

            }
            TempData["ErrorMessage"] = "Failed to create user!";
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


        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDto userDto)
        {
            /* ModelState IsValid when
             * if user name not less than 4 char with no space
             * if password valid - at least(8 characters - one digit - one upper case - one lower case - one special character)
             */
            if (ModelState.IsValid)
            {
                // Log in user
                if (await _authenticationService.Login(userDto) is LoginStatus.LoggedInSuccessfully)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // If ModelState is not valid or login was unsuccessful, set an error message
            TempData["ErrorMessage"] = "Invalid username or password.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await _authenticationService.Logout();

            // Redirect the user to the home page or a different page
            return RedirectToAction("Login", "User");
        }

    }
}
