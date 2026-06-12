namespace Energy.Application.System.Services;

/// <summary>
/// Runs the idempotent system seeders that bring the database to a fully usable
/// state (schema top-ups, permission catalog, roles, users, menus, API endpoint
/// catalog and localization). Safe to invoke repeatedly.
/// </summary>
public interface ISystemSeeder
{
    /// <summary>
    /// Executes every seeding step in order. Existing data is preserved; only
    /// missing rows are added and convergent values are updated.
    /// </summary>
    Task SeedAllAsync(CancellationToken cancellationToken = default);
}

