using System.ComponentModel.DataAnnotations;

namespace TwoFactorAuthentication.Dto.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Please enter a username.")]
        //At least 4 characters - no space
        [RegularExpression(@"^(?!.*\s).{4,}$", ErrorMessage = "Username must be at least 4 characters long and cannot contain spaces.")]
        public required string Username { get; set; }


        [Required(ErrorMessage = "Please enter a password.")]
        //at least (8 characters - one digit - one upper case - one lower case - one special character)
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$",
         ErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public required string Password { get; set; }

        //TODO: Take Token of user who create
        public required string Token { get; set; }
    }
}
