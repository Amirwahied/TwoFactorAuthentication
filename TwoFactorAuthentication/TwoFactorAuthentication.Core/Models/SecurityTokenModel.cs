
namespace TwoFactorAuthentication.Core.Models
{
    public class SecurityTokenModel
    {
        public required string EncryptionKey { get; set; }

        //The initialization vector is used to ensure that the same plaintext encrypted with the same key results in different ciphertexts.
        //This is important for security because if the same plaintext always produced the same ciphertext, it would be susceptible to various attacks,
        //including frequency analysis and pattern recognition.
        public required string InitializationVector { get; set; }
    }
}
