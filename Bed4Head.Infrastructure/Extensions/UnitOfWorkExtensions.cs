using Bed4Head.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bed4Head.Infrastructure.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static void AddUnitOfWorkService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

    }
}



