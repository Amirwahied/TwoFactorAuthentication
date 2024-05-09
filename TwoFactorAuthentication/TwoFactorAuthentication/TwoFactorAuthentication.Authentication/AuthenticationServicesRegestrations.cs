using Microsoft.Extensions.DependencyInjection;
using TwoFactorAuthentication.Authentication.Contracts.Repositories;
using TwoFactorAuthentication.Authentication.Implementations.Repositories;


namespace TwoFactorAuthentication.Authentication
{
    public static class AuthenticationServicesRegestrations
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

    }
}
