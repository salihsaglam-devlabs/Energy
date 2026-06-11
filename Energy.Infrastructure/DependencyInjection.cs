using Energy.Application.Home.Services;
using Energy.Application.Identity.Services;
using Energy.Application.Localization.Services;
using Energy.Application.Logger.Services;
using Energy.Application.System.Services;
using Energy.Infrastructure.Home.Services;
using Energy.Infrastructure.Identity;
using Energy.Infrastructure.Identity.Services;
using Energy.Infrastructure.Localization;
using Energy.Infrastructure.Logger.Services;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Seeding;
using Energy.Infrastructure.System.Services;
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
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException(
                                   LocalizationText.Get(
                                       LocalizationKeys.Messages.DefaultConnectionNotConfigured,
                                       "DefaultConnection is not configured."));

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<LocalizationSettings>(configuration.GetSection(LocalizationSettings.SectionName));

        services.AddScoped<PasswordHashingService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IAccessRuleService, AccessRuleService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IHomeService, HomeService>();

        services.AddLocalizationOverrides();

        services.AddScoped<SystemSeeder>();

        return services;
    }

    /// <summary>
    /// Replaces the default resx-only string localizer factory with one that
    /// checks the database first and falls back to the .resx files, and
    /// registers the writable <see cref="ILocalizationService"/> facade.
    /// </summary>
    private static IServiceCollection AddLocalizationOverrides(this IServiceCollection services)
    {
        services.AddSingleton<LocalizationCache>();
        services.AddSingleton<ResxFileWriter>();
        services.AddScoped<ILocalizationService, DatabaseLocalizationService>();

        // The wrapper factory needs the concrete framework factory to delegate to.
        services.AddSingleton<ResourceManagerStringLocalizerFactory>();
        services.AddSingleton<IStringLocalizerFactory, DbStringLocalizerFactory>();

        return services;
    }
}
