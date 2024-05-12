namespace TwoFactorAuthentication.Authentication.Enums
{
    public enum LoginStatus
    {
        InvalidUsernameOrPassword,
        LoggedInSuccessfully,
        LoginFailed,
        DatabaseError,
        TwoFactorRequired
    }
}
