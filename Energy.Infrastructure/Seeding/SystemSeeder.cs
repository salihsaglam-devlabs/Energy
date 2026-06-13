using Energy.Application.Identity.Services;
using Energy.Application.Localization.Services;
using Energy.Application.System.Services;
using Energy.Domain.Identity;
using Energy.Domain.System;
using Energy.Infrastructure.Identity.Services;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.System.Services;
using Energy.Localization;
using Energy.Shared.Identity;
using Energy.Shared.Identity.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Idempotent başlangıç tohumlayıcısı. Veritabanını tamamen kullanılabilir bir
/// duruma getirir: yetki kataloğu, SuperAdmin + admin kullanıcısı, temel menü ağacı,
/// varsayılan yetki eşlemesiyle API uç noktası kataloğu, yerelleştirme içe aktarımı
/// ve yeni mimarinin desteklediği yaygın kullanım kalıplarını kapsayan, özenle
/// hazırlanmış örnek roller + demo kullanıcılar kataloğu. Her adım yeniden
/// çalıştırılmaya güvenlidir.
///
/// <para>
/// <see cref="SeedAllAsync"/>'i yukarıdan aşağıya okuyun: sistemin ilişki sırasını
/// izler; böylece bir geliştirici, verilerin birbirine bağımlı olduğu sırayla, ilgili
/// aşamaya ekleme yaparak onu genişletebilir:
/// </para>
/// <list type="number">
///   <item>Şema tamamlamaları (önceden var olan veritabanları için geçiş içermeyen DDL).</item>
///   <item>Yetki kataloğu senkronizasyonu — merkezi, tip güvenli
///         <see cref="PermissionCatalog"/>'u okur ve veritabanında henüz olmayan her
///         kodu ekler (mevcut satırlar yenilenir, asla silinmez); böylece uygulamada
///         herhangi bir yerde kullanılan hiçbir yetki eksik kalmaz.</item>
///   <item>Roller (SuperAdmin + <see cref="SampleRoles"/> şablonları).</item>
///   <item>Rol → yetki eşlemeleri (şablon başına + varsayılan taban).</item>
///   <item>Menüler + API uç noktası kataloğu (her biri bir katalog yetkisiyle korunur).</item>
///   <item>Varsayılan kullanıcılar (admin, servis hesabı, şablon başına demo kullanıcılar).</item>
///   <item>Yerelleştirme içe aktarımı.</item>
/// </list>
/// <para>
/// Yeni bir yetenek eklemek için: yetkilerini <see cref="PermissionCatalog"/> içinde
/// tanımlayın, isteğe bağlı olarak <see cref="SampleRoles"/> içindeki bir rol şablonuna
/// ve bir menü/uç noktaya eşleyin — tohumlamanın geri kalanı bir sonraki başlangıçta
/// otomatik çalışır.
/// </para>
/// </summary>
public sealed partial class SystemSeeder : ISystemSeeder
{
    /// <summary>
    /// Kullanıcı arketiplerinin referans kataloğu. Her girdi bir rolü, sahip olduğu
    /// tam yetki kodu kümesine ve bu rolü taşıyan demo kullanıcıya eşler. Yöneticiler
    /// bunları Roller ekranından şablon olarak kopyalayabilir.
    /// </summary>
    private sealed record SampleRole(
        string RoleName,
        string RoleDescription,
        IReadOnlyList<string> PermissionCodes,
        SampleUser? DemoUser);

    /// <summary>Bir demo kullanıcısının tohumlama bilgilerini taşıyan kayıt.</summary>
    private sealed record SampleUser(
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password);

    /// <summary>
    /// Yerleşik rol şablonları. SuperAdmin ayrı ele alınır; çünkü yetki kontrollerini
    /// atlar ve burada görünmez.
    /// </summary>
    private static readonly IReadOnlyList<SampleRole> SampleRoles =
    [
        // ---------------- BT / Platform yönetimi ----------------
        // Yetki tabanlı tam yönetici: katalogdaki HER yetkiye sahiptir.
        // SuperAdmin'den farklıdır (o, kontrolleri atlar ve sisteme kilitlidir);
        // SystemAdmin, Roller ekranından tamamen yönetilebilir olmasına rağmen her
        // modülü kapsar — böylece katalogda atanmamış başıboş yetki kalmaz.
        new(
            RoleName: "SystemAdmin",
            RoleDescription: LocalizationKeys.RoleSeed.SystemAdminDescription,
            PermissionCodes: [.. PermissionCatalog.All.Select(p => p.Code)],
            DemoUser: new SampleUser("system.admin", "system.admin@energy.local", "Selin", "Aydın", "SysAdmin123!")),

        // ---------------- Operasyonel yönetim (güvenlik işlemleri yok) ----------------
        new(
            RoleName: "OperationsManager",
            RoleDescription: LocalizationKeys.RoleSeed.OperationsManagerDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.UserReadAll, PermissionCatalog.UserRead,
                PermissionCatalog.UserCreate, PermissionCatalog.UserUpdate,
                PermissionCatalog.RoleReadAll, PermissionCatalog.RoleRead,
                PermissionCatalog.MenuReadAll, PermissionCatalog.MenuRead,
                PermissionCatalog.MenuCreate, PermissionCatalog.MenuUpdate, PermissionCatalog.MenuDelete,
                PermissionCatalog.LogReadAll, PermissionCatalog.LogRead,
            ],
            DemoUser: new SampleUser("ops.manager", "ops.manager@energy.local", "Mert", "Yıldız", "OpsMgr123!")),

        // ---------------- Güvenlik / uyumluluk ----------------
        new(
            RoleName: "SecurityAuditor",
            RoleDescription: LocalizationKeys.RoleSeed.SecurityAuditorDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.UserReadAll, PermissionCatalog.UserRead,
                PermissionCatalog.RoleReadAll, PermissionCatalog.RoleRead,
                PermissionCatalog.PermissionReadAll, PermissionCatalog.PermissionRead,
                PermissionCatalog.ApiAccessReadAll, PermissionCatalog.ApiAccessRead,
                PermissionCatalog.MenuReadAll, PermissionCatalog.MenuRead,
                PermissionCatalog.LogReadAll, PermissionCatalog.LogRead,
            ],
            DemoUser: new SampleUser("security.auditor", "security.auditor@energy.local", "Deniz", "Kaya", "Auditor123!")),

        // ---------------- Çeviri / içerik ----------------
        new(
            RoleName: "LocalizationEditor",
            RoleDescription: LocalizationKeys.RoleSeed.LocalizationEditorDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.LocalizationReadAll, PermissionCatalog.LocalizationRead,
                PermissionCatalog.LocalizationCreate, PermissionCatalog.LocalizationUpdate, PermissionCatalog.LocalizationDelete,
            ],
            DemoUser: new SampleUser("localization.editor", "localization.editor@energy.local", "Elif", "Demir", "Editor123!")),

        // ---------------- Raporlama / yalnızca görüntüleme ----------------
        new(
            RoleName: "ReadOnlyViewer",
            RoleDescription: LocalizationKeys.RoleSeed.ReadOnlyViewerDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
                PermissionCatalog.UserReadAll, PermissionCatalog.UserRead,
                PermissionCatalog.RoleReadAll, PermissionCatalog.RoleRead,
                PermissionCatalog.PermissionReadAll, PermissionCatalog.PermissionRead,
                PermissionCatalog.MenuReadAll, PermissionCatalog.MenuRead,
                PermissionCatalog.ApiAccessReadAll, PermissionCatalog.ApiAccessRead,
                PermissionCatalog.LocalizationReadAll, PermissionCatalog.LocalizationRead,
                PermissionCatalog.LogReadAll, PermissionCatalog.LogRead,
            ],
            DemoUser: new SampleUser("readonly.viewer", "readonly.viewer@energy.local", "Ayşe", "Çelik", "Viewer123!")),

        // ---------------- Minimum baseline employee ----------------
        new(
            RoleName: "BasicUser",
            RoleDescription: LocalizationKeys.RoleSeed.BasicUserDescription,
            PermissionCodes:
            [
                PermissionCatalog.DashboardRead,
            ],
            DemoUser: new SampleUser("basic.user", "basic.user@energy.local", "Ahmet", "Şahin", "Basic123!")),
    ];

    private readonly AppDbContext _db;
    private readonly IPermissionService _permissions;
    private readonly IPermissionResolver _permissionResolver;
    private readonly ApiEndpointSyncService _endpointSync;
    private readonly ILocalizationService _localization;
    private readonly PasswordHashingService _passwords;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SystemSeeder> _logger;

    public SystemSeeder(
        AppDbContext db,
        IPermissionService permissions,
        IPermissionResolver permissionResolver,
        ApiEndpointSyncService endpointSync,
        ILocalizationService localization,
        PasswordHashingService passwords,
        IConfiguration configuration,
        ILogger<SystemSeeder> logger)
    {
        _db = db;
        _permissions = permissions;
        _permissionResolver = permissionResolver;
        _endpointSync = endpointSync;
        _localization = localization;
        _passwords = passwords;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Tüm tohumlama adımlarını yukarıdan aşağıya doğru sırayla çalıştırır.</summary>
    public async Task SeedAllAsync(CancellationToken ct = default)
    {
        // 1) ŞEMA — veritabanını (yeni veya mevcut) güncel modele taşır. Tüm
        //    idempotent, geçiş içermeyen DDL, SystemSeeder.Schema.cs partial dosyasında
        //    yer alır; böylece bu metot okunabilir, yukarıdan aşağıya bir VERİ anlatısı
        //    olarak kalır.
        await EnsureSchemaAsync(ct);

        // 2) YETKİLER — merkezi, tip güvenli PermissionCatalog'u veritabanına yansıtır.
        //    Yalnızca veritabanında eksik olan kodlar eklenir (mevcut satırlar
        //    yenilenir, asla silinmez); böylece uygulamanın herhangi bir yerinde
        //    kullanılan her yetki her zaman mevcut ve yönetilebilir olur.
        _logger.LogInformation("Seeding: permission catalog");
        var permissionsAdded = await _permissions.SyncCatalogAsync(ct);
        _logger.LogInformation("Seeding: {Added} permission(s) added to catalog", permissionsAdded);

        // 3) ROLLER + kullanıcıları — SuperAdmin (sistem) ve servis hesabı.
        _logger.LogInformation("Seeding: SuperAdmin role + admin user");
        await EnsureSuperAdminAsync(ct);

        _logger.LogInformation("Seeding: non-interactive system service account");
        await EnsureSystemServiceAccountAsync(ct);

        // 4) MENÜLER + API UÇ NOKTALARI — her biri bir katalog yetki koduyla korunur.
        _logger.LogInformation("Seeding: baseline menu tree");
        await EnsureBaselineMenusAsync(ct);

        _logger.LogInformation("Seeding: API endpoint discovery + default permission mapping");
        await _endpointSync.SyncAsync(ct);

        // 5) ROL → YETKİ eşlemeleri + örnek rol/kullanıcı şablonları.
        _logger.LogInformation("Seeding: sample role templates + demo users");
        await EnsureSampleRolesAndUsersAsync(ct);

        _logger.LogInformation("Seeding: default permission grants for every role");
        await EnsureDefaultPermissionsForAllRolesAsync(ct);

        // 6) YERELLEŞTİRME — resx (geliştirme) + gömülü kaynaklar (üretim) veritabanına.
        _logger.LogInformation("Seeding: localization resources (resx → DB)");
        var localizationResult = await _localization.ImportFromResxAsync(ct);
        _logger.LogInformation(
            "Localization (resx): {Added} added, {Updated} updated, {Total} total entries.",
            localizationResult.Added, localizationResult.Updated, localizationResult.Total);

        // Gömülü kaynak tohumlaması koşulsuz çalışır; böylece kaynak .resx dosyaları
        // diskte bulunmasa bile (üretimde olduğu gibi) veritabanı doldurulur.
        _logger.LogInformation("Seeding: localization resources (embedded → DB)");
        var embeddedResult = await _localization.SeedFromResourcesAsync(ct);
        _logger.LogInformation(
            "Localization (embedded): {Added} added, {Updated} updated, {Total} total entries.",
            embeddedResult.Added, embeddedResult.Updated, embeddedResult.Total);
    }

    /// <summary>SuperAdmin rolünün ve varsayılan yönetici kullanıcısının var olmasını sağlar.</summary>
    private async Task EnsureSuperAdminAsync(CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == SystemRoles.SuperAdmin, ct);
        if (role is null)
        {
            role = new Role
            {
                Id = Guid.NewGuid(),
                Name = SystemRoles.SuperAdmin,
                Description = LocalizationKeys.RoleSeed.SuperAdminDescription,
                IsSystem = true
            };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(ct);
        }

        const string adminEmail = "admin@energy.local";
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == adminEmail, ct);
        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                PasswordHash = _passwords.Hash("Admin123!"),
                IsActive = true,
                SecurityStamp = Guid.NewGuid()
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
        {
            _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
            await _db.SaveChangesAsync(ct);
        }
    }

    /// <summary>
    /// Etkileşimsiz bir sistem/servis hesabının var olmasını ve SuperAdmin rolünün
    /// atanmasını (böylece her yetki kontrolünü atlamasını) sağlar. Dahili/sistem
    /// süreçleri (ör. anonim istekleri denetleyen Web katmanı), oturum açmış herhangi
    /// bir kullanıcıdan bağımsız olarak API uç noktalarına ulaşmak için bu hesapla
    /// kimlik doğrular. Parola yapılandırmadan ("ServiceAccount:Password") alınır ve
    /// her zaman yeniden uygulanır; böylece bir rotasyondan sonra API ve Web katmanları
    /// senkronize kalır.
    /// </summary>
    private async Task EnsureSystemServiceAccountAsync(CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == SystemRoles.SuperAdmin, ct);
        if (role is null)
        {
            // EnsureSuperAdminAsync önce çalışır, bu yüzden bu durum asla oluşmamalıdır.
            _logger.LogWarning("Service account seeding skipped: SuperAdmin role is missing.");
            return;
        }

        var password = _configuration[ServiceAccount.ApiPasswordConfigKey];
        if (string.IsNullOrWhiteSpace(password)) password = ServiceAccount.DefaultPassword;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == ServiceAccount.Email, ct);
        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                UserName = ServiceAccount.UserName,
                Email = ServiceAccount.Email,
                FirstName = ServiceAccount.FirstName,
                LastName = ServiceAccount.LastName,
                PasswordHash = _passwords.Hash(password),
                IsActive = true,
                SecurityStamp = Guid.NewGuid()
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Service account '{UserName}' created.", ServiceAccount.UserName);
        }
        else
        {
            // Yapılandırılmış parolayı / etkin durumu yeniden uygula; böylece gizli
            // anahtar değiştirilse veya hesap kilitlense bile Web katmanı her zaman giriş yapabilir.
            var changed = false;
            if (!_passwords.Verify(password, user.PasswordHash))
            {
                user.PasswordHash = _passwords.Hash(password);
                user.SecurityStamp = Guid.NewGuid();
                changed = true;
            }
            if (!user.IsActive) { user.IsActive = true; changed = true; }
            if (user.LockoutEnd is not null) { user.LockoutEnd = null; changed = true; }
            if (user.FailedLoginCount != 0) { user.FailedLoginCount = 0; changed = true; }
            if (changed)
            {
                await _db.SaveChangesAsync(ct);
                _permissionResolver.InvalidateUser(user.Id);
                _logger.LogInformation("Service account '{UserName}' credentials re-asserted.", ServiceAccount.UserName);
            }
        }

        if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
        {
            _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
            await _db.SaveChangesAsync(ct);
        }
    }

    /// <summary>Temel menü ağacını idempotent şekilde oluşturur/günceller.</summary>
    private async Task EnsureBaselineMenusAsync(CancellationToken ct)
    {
        // NameKey anahtarıyla düğüm bazlı idempotent upsert. Yeni eklenen ekranlar
        // (ör. Profil), yönetici düzenlemelerini silmeden veya mevcut ağacı yeniden
        // sıralamadan sonraki bir çalıştırmada eklenir.
        var system = await EnsureMenuAsync(LocalizationKeys.Menus.System, null, null, "preferences", 10, null, ct);

        // Kimliği doğrulanmış her kullanıcının eriştiği kullanıcı bazlı sayfalar
        // (yetkiler DefaultGrants kümesinin parçasıdır; böylece menü her zaman görünür).
        await EnsureMenuAsync(LocalizationKeys.Menus.Dashboard, null, "/dashboard", "home", 1, PermissionCatalog.DashboardRead, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Profile, null, "/profile", "user", 2, PermissionCatalog.ProfileRead, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Chat, null, "/chat", "message", 3, PermissionCatalog.ChatUse, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Settings, null, "/settings", "preferences", 4, PermissionCatalog.UserSettingsRead, ct);

        // Sistem yönetimi alt menüsü — referans projenin hiyerarşisini yansıtır
        // (her yönetici ekranı için bir girdi); her biri ilgili sayfanın/uç noktanın
        // gerektirdiği aynı yetki koduyla korunur.
        await EnsureMenuAsync(LocalizationKeys.Menus.Users, system.Id, "/users", "group", 11, PermissionCatalog.UserReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.UserAccess, system.Id, "/user-access", "card", 12, PermissionCatalog.UserUpdate, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Roles, system.Id, "/roles", "accountbox", 13, PermissionCatalog.RoleReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Permissions, system.Id, "/permissions", "key", 14, PermissionCatalog.PermissionReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Menus_, system.Id, "/menus", "menu", 15, PermissionCatalog.MenuReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.ApiEndpoints, system.Id, "/api-endpoints", "globe", 16, PermissionCatalog.ApiAccessReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Localization, system.Id, "/localization", "globe", 17, PermissionCatalog.LocalizationReadAll, ct);
        await EnsureMenuAsync(LocalizationKeys.Menus.Logs, system.Id, "/logs", "clock", 18, PermissionCatalog.LogReadAll, ct);
    }

    /// <summary>Verilen anahtara göre bir menü düğümünü idempotent şekilde oluşturur/günceller.</summary>
    private async Task<Menu> EnsureMenuAsync(
        string nameKey, Guid? parentId, string? url, string? icon, int order, string? requiredPermission, CancellationToken ct)
    {
        var menu = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == nameKey, ct);
        if (menu is not null)
        {
            // Temel yapıyı (hiyerarşi, bağlantı, ikon, sıra ve yetki) güncel tanıma
            // yakınsa; böylece mevcut veritabanları, düğümün kimliğini/anahtarını
            // kaybetmeden güncel menü ağacını benimser.
            var changed =
                menu.ParentId != parentId ||
                menu.Url != url ||
                menu.Icon != icon ||
                menu.DisplayOrder != order ||
                menu.RequiredPermissionCode != requiredPermission ||
                !menu.IsActive ||
                !menu.IsVisible;

            if (changed)
            {
                menu.ParentId = parentId;
                menu.Url = url;
                menu.Icon = icon;
                menu.DisplayOrder = order;
                menu.RequiredPermissionCode = requiredPermission;
                menu.IsActive = true;
                menu.IsVisible = true;
                await _db.SaveChangesAsync(ct);
            }
            return menu;
        }

        menu = new Menu
        {
            Id = Guid.NewGuid(),
            ParentId = parentId,
            NameKey = nameKey,
            Url = url,
            Icon = icon,
            DisplayOrder = order,
            RequiredPermissionCode = requiredPermission
        };
        _db.Menus.Add(menu);
        await _db.SaveChangesAsync(ct);
        return menu;
    }

    /// <summary>
    /// <see cref="PermissionCatalog.DefaultGrants"/> tabanını (gösterge panosu +
    /// self servis profil), SuperAdmin dışındaki her role verir (SuperAdmin yetki
    /// kontrollerini atlar). Herhangi bir role sahip her kullanıcının, açık atama
    /// olmadan gösterge panosuna ve kendi profiline her zaman ulaşabilmesini garanti eder.
    /// </summary>
    private async Task EnsureDefaultPermissionsForAllRolesAsync(CancellationToken ct)
    {
        var roles = await _db.Roles
            .Where(r => r.Name != SystemRoles.SuperAdmin)
            .ToListAsync(ct);

        var added = 0;
        foreach (var role in roles)
        {
            var existing = (await _db.RolePermissions
                    .Where(rp => rp.RoleId == role.Id)
                    .Select(rp => rp.PermissionCode)
                    .ToListAsync(ct))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var changed = false;
            foreach (var code in PermissionCatalog.DefaultGrants)
            {
                if (existing.Contains(code)) continue;
                _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionCode = code });
                added += 1;
                changed = true;
            }

            if (changed)
            {
                await _db.SaveChangesAsync(ct);
                await _permissionResolver.InvalidateRoleAsync(role.Id, ct);
            }
        }

        _logger.LogInformation("Default grants: {Added} default permission link(s) ensured across {Roles} role(s).", added, roles.Count);
    }

    private async Task EnsureSampleRolesAndUsersAsync(CancellationToken ct)
    {
        var rolesAdded = 0;
        var usersAdded = 0;
        var permissionLinks = 0;

        foreach (var sample in SampleRoles)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == sample.RoleName, ct);
            if (role is null)
            {
                role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = sample.RoleName,
                    Description = sample.RoleDescription,
                    IsSystem = false
                };
                _db.Roles.Add(role);
                await _db.SaveChangesAsync(ct);
                rolesAdded += 1;
            }

            // Yetki kümesini ekleyerek senkronize et — bir yöneticinin eklemiş
            // olabileceği yetkileri asla kaldırma.
            var existing = await _db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionCode)
                .ToListAsync(ct);
            var existingSet = existing.ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var code in sample.PermissionCodes.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (existingSet.Contains(code)) continue;
                _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionCode = code });
                permissionLinks += 1;
            }
            if (permissionLinks > 0) await _db.SaveChangesAsync(ct);

            // Demo kullanıcıyı bir kez sağla ve role bağla.
            if (sample.DemoUser is { } demo)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == demo.Email, ct);
                if (user is null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = demo.UserName,
                        Email = demo.Email,
                        FirstName = demo.FirstName,
                        LastName = demo.LastName,
                        PasswordHash = _passwords.Hash(demo.Password),
                        IsActive = true,
                        SecurityStamp = Guid.NewGuid()
                    };
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync(ct);
                    usersAdded += 1;
                }

                if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
                {
                    _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                    await _db.SaveChangesAsync(ct);
                    _permissionResolver.InvalidateUser(user.Id);
                }
            }
        }

        _logger.LogInformation(
            "Sample catalog: {Roles} role(s) added, {Users} demo user(s) added, {Links} permission link(s) added.",
            rolesAdded, usersAdded, permissionLinks);
    }
}
