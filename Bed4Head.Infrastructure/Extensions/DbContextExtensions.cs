using Bed4Head.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Bed4Head.Infrastructure.Extensions
{
    public static class DbContextExtensions 
    {
        public static void AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options
                    .UseNpgsql(connectionString)
                    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));
        }
    }
}


