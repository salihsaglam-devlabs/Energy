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
        // Veritabanı sağlayıcısı "Database:Provider" ile seçilebilir
        // (PostgreSql | SqlServer). Mevcut dağıtımlarla geriye dönük uyumluluk
        // için varsayılan olarak PostgreSQL kullanılır.
        var useSqlServer = IsSqlServerProvider(configuration["Database:Provider"]);

        // Seçilen sağlayıcıyla eşleşen bağlantı dizesini seç; böylece kullanıcı yalnızca
        // "Database:Provider" değerini değiştirir — her iki dize de önceden yapılandırılmıştır.
        // Sağlayıcıya özel anahtar yoksa "DefaultConnection"a geri düşer.
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
                // SQL Server migration'ları kendi derlemesinde yer alır.
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly("Energy.Migrations.SqlServer"));
            }
            else
            {
                // PostgreSQL migration'ları kendi derlemesinde yer alır; böylece iki
                // sağlayıcı tek bir ModelSnapshot'ı paylaşmaz.
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
        services.AddScoped<Application.Settings.Services.IUserSettingsService, Infrastructure.Settings.Services.UserSettingsService>();

        // Kurumsal modüllerin ortak CRUD servisi (açık generic kayıt).
        services.AddScoped(typeof(Application.Common.Crud.IGenericCrudService<>), typeof(Infrastructure.Common.GenericCrudService<>));

        // Ana-detay ekranları için ortak alt-koleksiyon sorgu servisi.
        services.AddScoped<Application.Common.Crud.IModuleDetailQueryService, Infrastructure.Common.ModuleDetailQueryService>();

        // Ana-detay ekranları için ortak alt-koleksiyon yazma (CRUD) servisi.
        services.AddScoped<Application.Common.Crud.IModuleDetailCommandService, Infrastructure.Common.ModuleDetailCommandService>();

        // Workflow (onay) motoru + kaynak belge durum güncelleyici.
        services.AddScoped<Application.Workflow.Services.IApprovalSourceUpdater, Infrastructure.Workflow.Services.ApprovalSourceUpdater>();
        services.AddScoped<Application.Workflow.Services.IApprovalWorkflowService, Infrastructure.Workflow.Services.ApprovalWorkflowService>();

        // Inventory FIFO çekirdeği + Procurement mal kabul iş kuralı.
        services.AddScoped<Application.Inventory.Services.IInventoryService, Infrastructure.Inventory.Services.InventoryService>();
        services.AddScoped<Application.Procurement.Services.IGoodsReceiptService, Infrastructure.Procurement.Services.GoodsReceiptService>();

        // Finance: allocation + puantaj/hakediş/bütçe iş kuralları.
        services.AddScoped<Application.Finance.Services.IFinanceService, Infrastructure.Finance.Services.FinanceService>();

        // Operations (iş emri) + Catalog (malzeme) iş kuralları.
        services.AddScoped<Application.Operations.Services.IWorkOrderService, Infrastructure.Operations.Services.WorkOrderService>();
        services.AddScoped<Application.Catalog.Services.IMaterialService, Infrastructure.Catalog.Services.MaterialService>();

        services.AddLocalizationOverrides();
        services.AddScoped<SystemSeeder>();
        services.AddScoped<ISystemSeeder>(sp => sp.GetRequiredService<SystemSeeder>());

        return services;
    }

    /// <summary>
    /// Yapılandırılan değerden SQL Server sağlayıcısını tanır. Yaygın takma adları
    /// kabul eder; böylece "SqlServer", "MsSql", "SQL Server" hepsi SQL Server'ı seçer;
    /// başka her şey (null/boş dahil) PostgreSQL'e geri düşer.
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
