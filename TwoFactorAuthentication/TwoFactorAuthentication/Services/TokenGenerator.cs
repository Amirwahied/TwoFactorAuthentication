using System.Security.Cryptography;
using System.Text;
using TwoFactorAuthentication.Enums;

namespace TwoFactorAuthentication.Services
{
    public class TokenGenerator
    {
        // Generate a random token
        public static byte[] GenerateToken(Guid Id)
        {
            using SHA512 sha512 = SHA512.Create();
            {
                return sha512.ComputeHash(Encoding.UTF32.GetBytes(Id.ToString()));
            }
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


            return TokenValidationStatus.CorrectToken;
        }



    }
}
