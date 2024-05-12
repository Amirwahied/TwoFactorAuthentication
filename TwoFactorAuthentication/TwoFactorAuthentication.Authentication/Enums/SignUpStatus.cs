namespace TwoFactorAuthentication.Authentication.Enums
{
    public enum SignUpStatus
    {
        InvalidToken,
        SignedUpSuccessfully,
        SignedUpFailed,
        UsernameAlreadyUsed,
        DatabaseError
    }
}
