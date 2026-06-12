using Energy.Application.Home.Services;
using Energy.Application.Identity.Services;
using Energy.Application.Localization.Services;
using Energy.Application.Logger.Services;
using Energy.Application.System.Services;
using Energy.Application.Chat.Services;
using Energy.Infrastructure.Home.Services;
using Energy.Infrastructure.Identity;
using Energy.Infrastructure.Identity.Services;
using Energy.Infrastructure.Localization;
using Energy.Infrastructure.Logger.Services;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Persistence.Interceptors;
using Energy.Infrastructure.Seeding;
using Energy.Infrastructure.System.Services;
using Energy.Infrastructure.Chat.Services;
using Energy.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database provider is selectable via "Database:Provider"
        // (PostgreSql | SqlServer). Defaults to PostgreSQL for backwards
        // compatibility with existing deployments.
        var useSqlServer = IsSqlServerProvider(configuration["Database:Provider"]);

        // Pick the connection string that matches the chosen provider so the user
        // only flips "Database:Provider" — both strings are pre-configured. Falls
        // back to "DefaultConnection" when the provider-specific key is absent.
        var providerKey = useSqlServer ? "SqlServer" : "PostgreSql";
        var connectionString = configuration.GetConnectionString(providerKey)
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                $"No connection string configured for provider '{providerKey}'. " +
                $"Set ConnectionStrings:{providerKey} or ConnectionStrings:DefaultConnection.");

        services.AddMemoryCache();

        services.AddScoped<AuditingSaveChangesInterceptor>();
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            if (useSqlServer)
            {
                // SQL Server migrations live in their own assembly.
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly("Energy.Migrations.SqlServer"));
            }
            else
            {
                // PostgreSQL migrations live in their own assembly so the two
                // providers don't share a single ModelSnapshot.
                options.UseNpgsql(connectionString, npg => npg.MigrationsAssembly("Energy.Migrations.PostgreSql"));
            }
            options.AddInterceptors(sp.GetRequiredService<AuditingSaveChangesInterceptor>());
        });

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<LocalizationSettings>(configuration.GetSection(LocalizationSettings.SectionName));

        services.AddScoped<PasswordHashingService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPermissionResolver, PermissionResolver>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IApiEndpointService, ApiEndpointService>();
        services.AddScoped<ApiEndpointSyncService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IChatService, ChatService>();

        services.AddLocalizationOverrides();
        services.AddScoped<SystemSeeder>();
        services.AddScoped<ISystemSeeder>(sp => sp.GetRequiredService<SystemSeeder>());

        return services;
    }

    /// <summary>
    /// Recognises the SQL Server provider from the configured value. Accepts the
    /// common aliases so "SqlServer", "MsSql", "SQL Server" all select SQL Server;
    /// anything else (including null/empty) falls back to PostgreSQL.
    /// </summary>
    internal static bool IsSqlServerProvider(string? provider)
    {
        if (string.IsNullOrWhiteSpace(provider)) return false;
        var p = provider.Trim().Replace(" ", string.Empty).ToLowerInvariant();
        return p is "sqlserver" or "mssql" or "sql" or "mssqlserver";
    }

    private static IServiceCollection AddLocalizationOverrides(this IServiceCollection services)
    {
        services.AddSingleton<LocalizationCache>();
        services.AddSingleton<ResxFileWriter>();
        services.AddSingleton<EmbeddedResourceReader>();
        services.AddScoped<ILocalizationService, DatabaseLocalizationService>();
        services.AddSingleton<ResourceManagerStringLocalizerFactory>();
        services.AddSingleton<IStringLocalizerFactory, DbStringLocalizerFactory>();
        return services;
    }
}
