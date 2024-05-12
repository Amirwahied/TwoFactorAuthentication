using Microsoft.Extensions.DependencyInjection;
using TwoFactorAuthentication._2FA.Contracts;
using TwoFactorAuthentication._2FA.Services;

namespace TwoFactorAuthentication._2FA
{
	public static class TwoFactorAuthenticationServicesRegestration
	{
		public static IServiceCollection AddTwoFactorAuthenticationServices(this IServiceCollection services)
		{
			services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();


			return services;
		}

	}
}
