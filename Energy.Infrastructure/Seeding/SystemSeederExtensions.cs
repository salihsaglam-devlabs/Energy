using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

public static class SystemSeederExtensions
{
    public static async Task RunSystemSeedingAsync(this IHost host, CancellationToken ct = default)
    {
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SystemSeeder>>();
        try
        {
            var seeder = scope.ServiceProvider.GetRequiredService<SystemSeeder>();
            await seeder.SeedAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System seeding failed; aborting startup.");
            throw;
        }
    }
}
