using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using TwoFactorAuthentication.Core.Contracts;
using TwoFactorAuthentication.Core.Models;

namespace TwoFactorAuthentication.Core.Helper
{
    public class TokenEncryptor:ITokenEncryptor
    {
        public TokenEncryptor(IOptions<SecurityTokenModel> model)
        {
            Model = model.Value;
            _key = Encoding.UTF8.GetBytes(Model.EncryptionKey);
            _iv = Encoding.UTF8.GetBytes(Model.InitializationVector);
        }

        public SecurityTokenModel Model { get; }


        // Encryption key (32 bytes for AES-256)
        private static byte[] _key;

        // Initialization vector (IV) (16 bytes for AES)
        private static byte[] _iv;

        public string Encrypt(string textToEncrypt)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream
                            swEncrypt.Write(textToEncrypt);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string textToDecrypt)
        {
            byte[] cipherBytes = Convert.FromBase64String(textToDecrypt);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                // Create a decryptor to perform the stream transform
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream and place them in a string
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

        }

    }
}
