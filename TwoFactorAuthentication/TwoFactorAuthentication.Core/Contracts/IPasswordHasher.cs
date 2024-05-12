namespace TwoFactorAuthentication.Core.Contracts
{
    public interface IPasswordHasher
    {
        byte[] HashPasword(string password, out byte[] salt);
        bool VerifyPassword(string clearPassword, byte[] storedPassword, byte[] storedSalt);
    }
}
