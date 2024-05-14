using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwoFactorAuthentication.Core.Contracts;
using TwoFactorAuthentication.Core.Contracts.Repositories;
using TwoFactorAuthentication.Core.Helper;
using TwoFactorAuthentication.Core.Implementation;
using TwoFactorAuthentication.Core.Implementations.Repositories;
using TwoFactorAuthentication.Core.Models;

namespace TwoFactorAuthentication.Core
{
    public static class CoreServicesRegestration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenEncryptor, TokenEncryptor>();


            services.Configure<SecurityTokenModel>(configuration.GetSection("SecurityTokenModel"));
            return services;
        }
    }
}
