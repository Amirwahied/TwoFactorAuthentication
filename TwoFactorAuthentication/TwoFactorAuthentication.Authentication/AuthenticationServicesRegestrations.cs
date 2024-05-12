using Microsoft.Extensions.DependencyInjection;
using TwoFactorAuthentication.Authentication.Contracts.Repositories;
using TwoFactorAuthentication.Authentication.Contracts.Services;
using TwoFactorAuthentication.Authentication.Implementations.Repositories;
using TwoFactorAuthentication.Authentication.Services;


namespace TwoFactorAuthentication.Authentication
{
    public static class AuthenticationServicesRegestrations
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }

    }
}
