namespace TwoFactorAuthentication.Core.Enums
{
    public enum TokenValidationStatus
    {
        FormatException,
        InvalidLength,
        InvalidToken,
        CorrectToken
    }
}
