using TwoFactorAuthentication.Authentication.Models;
using TwoFactorAuthentication.Authentication.Contracts.Repositories;
using TwoFactorAuthentication.Authentication.Dto;
using TwoFactorAuthentication.Core.Enums;
using TwoFactorAuthentication.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace TwoFactorAuthentication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenEncryptor _tokenEncryptor;

        public UserController(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenEncryptor tokenEncryptor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenEncryptor = tokenEncryptor;
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(CreateUserDto user)
        {
            if (ModelState.IsValid)
            {
                //Create hashed password and salt
                var hashedPassword = _passwordHasher.HashPasword(user.Password, out var salt);

                var userId = Guid.NewGuid();
                //Create Token
                var token = _tokenEncryptor.Encrypt(userId.ToString());


                //TODO: Map Dto to Domain Entity and create new user
                var userEntity = new User_Login_Info()
                {
                    Id = userId,
                    Username = user.Username,
                    Password = hashedPassword,
                    Salt = salt,
                    Token = token,

                };
                var guid = _tokenEncryptor.Decrypt(token);

                if (await _userRepository.CreateAsync(userEntity,StoredProcedureNames.users_login_info_CreateUser.ToString()) > 0)
                {
                    //TODO: Successfully Message
                }
                else
                {
                    //TODO: Alert Message
                }

                return RedirectToAction("RegistrationSuccess");
            }
            else
            {
                // If username or password is empty, return to the signup page with an error message
                ViewBag.ErrorMessage = "Username and password are required.";
                return View();
            }
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDto user)
        {
            if (ModelState.IsValid)
            {
                //if user name not less than 4 char with no space

                //if password valid - at least(8 characters - one digit - one upper case - one lower case - one special character)

                //get user by username
                var currentUser = await _userRepository.GetByUsername(user.Username);

                //if not found - not valid username or password
                if (currentUser is null)
                {
                    //TODO: Alert Message - not valid username or password
                }

                //get salt
                //var salt = currentUser?.Salt;

                //hashed password
                var hashedPassword = _passwordHasher.HashPasword(user.Password, out var salt);

                //compare hashed password
                var result = _passwordHasher.VerifyPassword(user.Password, hashedPassword, salt);

                //if not valid (invalid password)
                if (!result)
                {
                    //TODO: Alert Message - not valid password
                }

                //if valid 

                /* set user active if not
                 * cache token in session
                 * open welcome page
                 */

                if (!currentUser!.Is_Active)
                {
                    if (await _userRepository.ActivateUser(currentUser.Id) > 0)
                    {
                        //Successfully activated
                    }
                }


                HttpContext.Session.SetString(currentUser.Username, currentUser.Token);

                //TODO: 
                /* Token = hashed GUID of user 
                 * Token Stored in session
                 */
            }
            else
            {
                //not valid username or password - confilct with validations
            }
            return View();

        }

    }
}
