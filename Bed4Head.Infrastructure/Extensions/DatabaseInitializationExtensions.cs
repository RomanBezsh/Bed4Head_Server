using Bed4Head.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Bed4Head.Infrastructure.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task EnsureDatabaseCreatedAndMigratedAsync(this IServiceProvider services, string connectionString, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string is missing.", nameof(connectionString));

            var csb = new NpgsqlConnectionStringBuilder(connectionString);
            var targetDatabase = csb.Database;

            if (string.IsNullOrWhiteSpace(targetDatabase))
                throw new InvalidOperationException("Connection string must specify a Database.");

            await EnsureDatabaseExistsAsync(csb, targetDatabase, cancellationToken);

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync(cancellationToken);
        }

        private static async Task EnsureDatabaseExistsAsync(NpgsqlConnectionStringBuilder csb, string targetDatabase, CancellationToken cancellationToken)
        {
            var adminCsb = new NpgsqlConnectionStringBuilder(csb.ConnectionString)
            {
                Database = "postgres"
            };

            await using var conn = new NpgsqlConnection(adminCsb.ConnectionString);
            await conn.OpenAsync(cancellationToken);

            await using (var existsCmd = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = @name;", conn))
            {
                existsCmd.Parameters.AddWithValue("name", targetDatabase);
                var exists = await existsCmd.ExecuteScalarAsync(cancellationToken);
                if (exists is not null)
                    return;
            }

            var safeName = targetDatabase.Replace("\"", "\"\"");
            await using var createCmd = new NpgsqlCommand($"CREATE DATABASE \"{safeName}\";", conn);
            await createCmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}

