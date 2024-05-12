namespace TwoFactorAuthentication._2FA.Model
{
    public class QrResponse
    {
        public required string ImageUrl { get; set; }
        public required string ManualKey { get; set; }
    }
}
