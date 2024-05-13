using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwoFactorAuthentication.Authentication.Constants;
namespace TwoFactorAuthentication.CustomAttribute
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }
    }

    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user != null)
            {
                bool is2FAVerified = Convert.ToBoolean(user?.FindFirst(CustomClaimsTypes.Is2FAVerified)?.Value);
                bool isTwoFactorEnabled = Convert.ToBoolean(user?.FindFirst(CustomClaimsTypes.IsTwoFactorEnabled)?.Value);

                if (!is2FAVerified && isTwoFactorEnabled || !user.Identity.IsAuthenticated)
                {
                    context.Result = new StatusCodeResult(404); // Or any other result you prefer
                }
            }
        }
    }
}
