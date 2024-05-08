using Microsoft.AspNetCore.Mvc;
using TwoFactorAuthentication.Contracts.Repositories;
using TwoFactorAuthentication.Dto.User;
using TwoFactorAuthentication.Models;
using TwoFactorAuthentication.Services;

namespace TwoFactorAuthentication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                var hashedPassword = PasswordHasher.HashPasword(user.Password, out var salt);

                var userId = Guid.NewGuid();
                //Create Token
                var token = TokenGenerator.GenerateToken(userId);


                //TODO: Map Dto to Domain Entity and create new user
                var userEntity = new User_Login_Info()
                {
                    Id = userId,
                    Username = user.Username,
                    Password = hashedPassword,
                    Salt = salt,
                    Token = token,

                };

                if (await _userRepository.CreateAsync(userEntity) > 0)
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

        public async Task<ActionResult> Login(LoginUserDto user)
        {
            if (ModelState.IsValid)
            {
                //if user name not less than 4 char with no space

                //if password valid - at least(8 characters - one digit - one upper case - one lower case - one special character)

                //get user by username

                //get salt

                //compare hashed password

                //if not valid (invalid username or password)

                //if valid 

                /* set user active if not
                 * cache token in session
                 * open welcome page
                 */


                //TODO: 
                /* Token = hashed GUID of user 
                 * Token Stored in session
                 */
            }
            else
            {
                //not valid username or password - confilct with validations
            }
        }

    }
}
