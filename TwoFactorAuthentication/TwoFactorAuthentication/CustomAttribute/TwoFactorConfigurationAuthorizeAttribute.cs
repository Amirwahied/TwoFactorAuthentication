using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwoFactorAuthentication.Authentication.Constants;
namespace TwoFactorAuthentication.CustomAttribute
{
    public class TwoFactorConfigurationAuthorizeAttribute : TypeFilterAttribute
    {
        public TwoFactorConfigurationAuthorizeAttribute() : base(typeof(TwoFactorConfigurationAuthorizeFilter))
        {
        }
    }

    public class TwoFactorConfigurationAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user != null)
            {
                bool isTwoFactorEnabled = Convert.ToBoolean(user?.FindFirst(CustomClaimsTypes.IsTwoFactorEnabled)?.Value);

                if (!isTwoFactorEnabled)
                {
                    context.Result = new StatusCodeResult(404); // Or any other result you prefer
                }
            }
        }
    }
}
