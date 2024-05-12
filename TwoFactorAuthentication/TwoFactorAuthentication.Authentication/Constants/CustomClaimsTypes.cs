using System.Security.Claims;

namespace TwoFactorAuthentication.Authentication.Constants
{
    public class CustomClaimsTypes
    {
        public const string UserID = ClaimTypes.NameIdentifier;
        public const string Username = ClaimTypes.Name;
        public const string Token = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/token";
    }
}
