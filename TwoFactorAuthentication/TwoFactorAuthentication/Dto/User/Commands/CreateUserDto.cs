namespace TwoFactorAuthentication.Dto.User.Commands
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }

        //TODO: Take Token of user who create
        //public required string Toekn { get; set; }
    }
}
