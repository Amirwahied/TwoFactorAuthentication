using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;
using TwoFactorAuthentication._2FA.Contracts;
using TwoFactorAuthentication.Authentication.Constants;
using TwoFactorAuthentication.Authentication.Contracts.Services;
using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Authentication.Enums;
using TwoFactorAuthentication.CustomAttribute;
using IAuthenticationService = TwoFactorAuthentication.Authentication.Contracts.Services.IAuthenticationService;

namespace TwoFactorAuthentication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManagementService userManagementService
                            , IAuthenticationService authenticationService
                            , ITwoFactorAuthService twoFactorAuthService
                            , IHttpContextAccessor httpContextAccessor
                            , ILogger<UserController> logger)
        {
            _userManagementService = userManagementService;
            _authenticationService = authenticationService;
            _twoFactorAuthService = twoFactorAuthService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        [CustomAuthorize]
        public IActionResult SignUp()
        {
            return View();
        }

        void LogError(string modelMessage, string logMessage)
        {
            ModelState.AddModelError("error", modelMessage);
            _logger.LogError(logMessage);
        }

        async Task UpdateClaim(string claim, string value)
        {
            // Retrieve the current authentication properties
            var authenticationProperties = await HttpContext.AuthenticateAsync();

            // Update the claims in the current identity
            var claimsIdentity = (ClaimsIdentity)authenticationProperties.Principal.Identity;
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(claim));
            // Add or update other claims as needed
            claimsIdentity.AddClaim(new Claim(claim, value));

            // Create a new claims principal with the updated identity
            var newClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            // Sign in the user with the updated authentication ticket
            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(newClaimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.MaxValue,
                    AllowRefresh = true
                });

            // Remove the original authentication ticket from the response
            HttpContext.Response.Cookies.Delete(
                CookieAuthenticationDefaults.CookiePrefix + CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [CustomAuthorize]
        [HttpPost]
        public async Task<ActionResult> SignUp(CreateUserDto userDto)
        {
            try
            {
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
                            ModelState.AddModelError("error", "Your token is invalid!");
                            break;
                    }
                }
                return View();
            }
            catch (SqlException ex)
            {
                //Username duplicated
                if (ex.Message.Contains("Cannot insert duplicate key"))
                {
                    LogError("Username already used!", $"Createing new user failed due to database error!\n{ex.Message}\n{ex.InnerException}");
                    return View();
                }
                LogError("Createing new user failed!", $"Createing new user failed due to database error!\n{ex.Message}\n{ex.InnerException}");
            }
            catch (Exception ex)
            {
                LogError("Createing new user failed!", $"Createing new user failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
                bool is2FAVerified = Convert.ToBoolean(((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.Is2FAVerified)?.Value);
                bool isTwoFactorEnabled = Convert.ToBoolean(((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.IsTwoFactorEnabled)?.Value);

                if (!is2FAVerified && isTwoFactorEnabled)
                {
                    await _httpContextAccessor.HttpContext.SignOutAsync();
                }
                else
                {
                    // If the user is already authenticated, redirect them to the home page
                    if (User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "");
                LogError("Login failed!", $"Createing new user failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDto userDto)
        {
            try
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
                            ModelState.AddModelError("error", "Invalid username or password.");
                            break;
                    }
                }

                // If ModelState is not valid or login was unsuccessful, set an error message
                TempData["ErrorMessage"] = errorMessage;
            }
            catch (SqlException ex)
            {
                LogError("Login failed!", $"Login failed due to database error!\n{ex.Message}\n{ex.InnerException}");
            }
            catch (Exception ex)
            {
                LogError("Login failed!", $"Login failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }

        [CustomAuthorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authenticationService.Logout();

                // Redirect the user to the home page or a different page
                return RedirectToAction(nameof(Login), "User");
            }
            catch (SqlException ex)
            {
                LogError("Logout failed!", $"Logout failed due to database error!\n{ex.Message}\n{ex.InnerException}");
            }
            catch (Exception ex)
            {
                LogError("Logout failed!", $"Logout failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }

        [CustomAuthorize]
        [HttpGet]
        public IActionResult Enable2FA()
        {
            return View();
        }

        [CustomAuthorize]
        [HttpPost]
        public async Task<IActionResult> Enable2FA(bool isEnabled2FA)
        {
            try
            {
                if (!isEnabled2FA)
                {
                    await _userManagementService.Enable2FA(new Guid(((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.UserID)?.Value));

                    await UpdateClaim(CustomClaimsTypes.IsTwoFactorEnabled, "True");


                    return RedirectToAction(nameof(TwoFactorAuthenticationConfiguration), "User");
                }
            }
            catch (SqlException ex)
            {
                LogError("Enable two factor authentication failed!", $"Enable two factor authentication failed due to database error!\n{ex.Message}\n{ex.InnerException}");
            }
            catch (Exception ex)
            {
                LogError("Enable two factor authentication failed!", $"Enable two factor authentication failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }

        [TwoFactorConfigurationAuthorize]
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthenticationConfiguration()
        {
            try
            {
                var qr = _twoFactorAuthService.GetQr(User.Identity.Name);

                _httpContextAccessor.HttpContext.Session.SetString("ImageUrl", qr.ImageUrl);
                _httpContextAccessor.HttpContext.Session.SetString("ManualKey", qr.ManualKey);

                await _userManagementService.UpdateAuthenticatorKey(new Guid(((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.UserID)?.Value), qr.ManualKey);

            }
            catch (SqlException ex)
            {
                LogError("Two factor authentication configuration failed!", $"Two factor authentication configuration due to database error!\n{ex.Message}\n{ex.InnerException}");
            }
            catch (Exception ex)
            {
                LogError("Two factor authentication configuration failed!", $"Two factor authentication configuration failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }

        [TwoFactorConfigurationAuthorize]
        [HttpPost]
        public async Task<IActionResult> TwoFactorAuthenticationConfiguration(string pincode)
        {
            try
            {
                if (_twoFactorAuthService.VerifyAuthentication(pincode, User.Identity.Name))
                {
                    await UpdateClaim(CustomClaimsTypes.Is2FAVerified, "True");

                    // Redirect the user to the home page or a different page
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ValidationResult"] = "Failed";
                    return RedirectToAction(nameof(TwoFactorAuthenticationConfiguration), "User");
                }
            }
            catch (Exception ex)
            {
                LogError("Two factor authentication configuration failed!", $"Two factor authentication configuration failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }


        [Authorize]
        [HttpGet]
        public IActionResult TwoFactorAuthenticationCheck()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TwoFactorAuthenticationCheck(string pincode)
        {
            try
            {
                if (_twoFactorAuthService.VerifyAuthentication(pincode, User.Identity.Name))
                {


                    // Retrieve the current authentication properties
                    var authenticationProperties = await HttpContext.AuthenticateAsync();

                    // Update the claims in the current identity
                    var claimsIdentity = (ClaimsIdentity)authenticationProperties.Principal.Identity;
                    claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(CustomClaimsTypes.Is2FAVerified));
                    // Add or update other claims as needed
                    claimsIdentity.AddClaim(new Claim(CustomClaimsTypes.Is2FAVerified, "True"));

                    // Create a new claims principal with the updated identity
                    var newClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Create a new authentication ticket with the updated claims principal
                    var newAuthenticationTicket = new AuthenticationTicket(
                        newClaimsPrincipal,
                        authenticationProperties.Properties,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user with the updated authentication ticket
                    //sign in
                    await _httpContextAccessor.HttpContext.SignInAsync(newClaimsPrincipal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.MaxValue,
                            AllowRefresh = true
                        });

                    // Remove the original authentication ticket from the response
                    HttpContext.Response.Cookies.Delete(
                        CookieAuthenticationDefaults.CookiePrefix + CookieAuthenticationDefaults.AuthenticationScheme);


                    // Redirect the user to the home page or a different page
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("error", "Verification failed!");
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogError("Verification failed!", $"Verification failed!\n{ex.Message}\n{ex.InnerException}");
            }
            return View();
        }


    }
}
