namespace TwoFactorAuthentication.ViewModels.User
{
    public sealed record class LoginViewModel
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Token { get; set; }

    }
}
