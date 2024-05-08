using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TwoFactorAuthentication.Contracts;
using TwoFactorAuthentication.Dto.User.Commands;
using TwoFactorAuthentication.Services;

namespace TwoFactorAuthentication.Controllers
{
    public class UserController : Controller
    {
        private readonly ISqlConnectionFactory _sqlConnection;

        public UserController(ISqlConnectionFactory sqlConnection)
        {
            _sqlConnection = sqlConnection;
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

                //Open SQL Connection
                await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();

                //Create Token
                var token = TokenGenerator.GenerateToken();

                sqlConnection.InsertAsync(user);
                return RedirectToAction("RegistrationSuccess");
            }
            else
            {
                // If username or password is empty, return to the signup page with an error message
                ViewBag.ErrorMessage = "Username and password are required.";
                return View();
            }
        }
    }
}
