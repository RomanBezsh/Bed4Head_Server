using Bed4Head.BLL.Interfaces;
using Bed4Head.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bed4Head.BLL.Extensions
{
    public static class AppServicesExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
