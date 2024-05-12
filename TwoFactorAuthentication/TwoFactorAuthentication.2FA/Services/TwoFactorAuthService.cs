using Google.Authenticator;
using Microsoft.Extensions.Options;
using System.Text;
using TwoFactorAuthentication._2FA.Contracts;
using TwoFactorAuthentication._2FA.Model;
using TwoFactorAuthentication.Core.Models;

namespace TwoFactorAuthentication._2FA.Services
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        private string _key; //Application Key
        private string UserUniquekey;
        public SecurityTokenModel Model { get; }

        public TwoFactorAuthService(IOptions<SecurityTokenModel> model)
        {
            Model = model.Value;
            _key = Model.EncryptionKey;
        }

        public QrResponse GetQr(string username)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

            var setupinfo = tfa.GenerateSetupCode("2FA", username.ToLower(), GetUserUniqueKeyInBytes(username), qrPixelsPerModule: 3);

            string ImageUrl = setupinfo.QrCodeSetupImageUrl;
            string Manualkey = setupinfo.ManualEntryKey;

            QrResponse response = new()
            {
                ImageUrl = ImageUrl,
                ManualKey = Manualkey
            };
            return response;
        }

        public bool VerifyAuthentication(string pin, string username)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

            return tfa.ValidateTwoFactorPIN(GetUserUniqueKeyInBytes(username), pin);
        }

        private byte[] GetUserUniqueKeyInBytes(string username)
        {
            UserUniquekey = username + _key;
            return Encoding.ASCII.GetBytes(UserUniquekey);
        }
    }
}
