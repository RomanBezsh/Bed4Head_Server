using Bed4Head.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bed4Head.BLL.Extensions
{
    public static class DbContextExtensions 
    {
        public static void AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
}