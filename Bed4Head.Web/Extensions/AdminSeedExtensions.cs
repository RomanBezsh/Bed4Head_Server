using Bed4Head.Domain.Enums;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bed4Head.Web.Extensions
{
    public static class AdminSeedExtensions
    {
        public static async Task EnsureAdminUsersAsync(this IServiceProvider services, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var emails = config.GetSection("Admin:SuperAdminEmails").Get<string[]>() ?? Array.Empty<string>();
            if (emails.Length == 0) return;

            using var scope = services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var users = await uow.Users.GetAllAsync();
            var byEmail = users.ToDictionary(u => u.Email, StringComparer.OrdinalIgnoreCase);

            var updated = false;
            foreach (var raw in emails)
            {
                var email = (raw ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(email)) continue;

                if (byEmail.TryGetValue(email, out var user) && user.Role != UserRole.Admin)
                {
                    user.Role = UserRole.Admin;
                    await uow.Users.UpdateAsync(user);
                    updated = true;
                }
            }

            if (updated)
            {
                await uow.CompleteAsync();
            }
        }
    }
}

