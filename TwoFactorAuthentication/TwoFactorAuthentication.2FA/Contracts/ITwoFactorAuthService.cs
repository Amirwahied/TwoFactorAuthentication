using TwoFactorAuthentication._2FA.Model;

namespace TwoFactorAuthentication._2FA.Contracts
{
    public interface ITwoFactorAuthService
    {
        QrResponse GetQr(string username);

        bool VerifyAuthentication(string pin, string username);
    }
}
