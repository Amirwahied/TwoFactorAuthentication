using System.Security.Cryptography;
using TwoFactorAuthentication.Enums;

namespace TwoFactorAuthentication.Services
{
    public class TokenGenerator
    {
        // Generate a random token
        public static string GenerateToken(int length = 64)
        {
            byte[] tokenBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes);
        }

        // Validate a token
        public static TokenValidationStatus ValidateToken(string token, int length = 64)
        {
            byte[] tokenBytes;

            // Check Token Validity
            try
            {
                tokenBytes = Convert.FromBase64String(token);
            }
            catch (FormatException)
            {
                return TokenValidationStatus.FormatException;
            }

            if (tokenBytes.Length != length)
            {
                return TokenValidationStatus.InvalidLength;
            }

            //TODO: Check Token Correcteness


            return true;
        }



    }
}
