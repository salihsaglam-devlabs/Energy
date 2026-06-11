using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

public static class SystemSeederExtensions
{
    /// <summary>
    /// Runs the <see cref="SystemSeeder"/> in a fresh DI scope, blocking until
    /// the database is ready. Failures are logged and rethrown so a misconfigured
    /// environment fails fast at startup instead of producing a half-seeded state.
    /// </summary>
    public static async Task RunSystemSeedingAsync(
        this IHost host,
        IReadOnlyCollection<string>? additionalPermissionCodes = null,
        CancellationToken cancellationToken = default)
    {
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SystemSeeder>>();

        try
        {
            var seeder = scope.ServiceProvider.GetRequiredService<SystemSeeder>();
            await seeder.SeedAsync(additionalPermissionCodes, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System seeding failed; the application will not start.");
            throw;
        }
    }
}

