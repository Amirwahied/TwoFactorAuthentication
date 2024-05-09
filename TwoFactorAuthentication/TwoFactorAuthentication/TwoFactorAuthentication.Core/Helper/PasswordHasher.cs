using System.Security.Cryptography;
using System.Text;
using TwoFactorAuthentication.Core.Contracts;

namespace TwoFactorAuthentication.Core.Helper
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int _keySize = 64; //the result of password and salt in bytes
        private const int _iterations = 350000; //Each iteration involves rehashing the output of the previous iteration along with the input password and salt
        private static HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public byte[] HashPasword(string password, out byte[] salt)
        {
            //Create salt from random bytes
            salt = RandomNumberGenerator.GetBytes(_keySize);

            return Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                _iterations,
                _hashAlgorithm,
                _keySize);
        }

        public bool VerifyPassword(string password, byte[] hashedPassword, byte[] salt)
        {

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithm, _keySize);

            //Timing Side-Channel Attacks
            //to ensures that the execution time remains constant to prevent guessing the correct charachter in password

            return CryptographicOperations.FixedTimeEquals(hashToCompare, hashedPassword);
        }
    }
}
