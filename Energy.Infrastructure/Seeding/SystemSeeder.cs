// ===================================================================================
// SystemSeeder - TUM seed verisinin TEK ve YAPILANDIRILMIS kaynagi.
// Sisteme nerede ne seed edildigi asagidaki #region bolumlerinden takip edilir.
// Pipeline sirasi icin SeedAllAsync (CEKIRDEK region) yukaridan asagiya okunur.
// Tum adimlar idempotent'tir (tekrar calistirma duplicate uretmez).
// ===================================================================================
using Energy.Application.Identity.Services;
using Energy.Application.Localization.Services;
using Energy.Application.System.Services;
using Energy.Domain.Assets;
using Energy.Domain.Budget;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Chat;
using Energy.Domain.Common;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.Documents;
using Energy.Domain.FieldOperations;
using Energy.Domain.Finance;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Notifications;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Procurement;
using Energy.Domain.ProgressPayments;
using Energy.Domain.Projects;
using Energy.Domain.Reporting;
using Energy.Domain.Requests;
using Energy.Domain.Workflow;
using Energy.Infrastructure.Identity.Services;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.System.Services;
using Energy.Localization;
using Energy.Shared.Common;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using BudgetEntity = Energy.Domain.Budget.Budget;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Idempotent baslangic tohumlayicisi: sema, yetki katalogu, roller/kullanicilar,
/// menuler, API uc noktalari, referans/master data, modul ornek verileri, 3 aylik
/// demo senaryosu ve dogrulama. Her bolum bir #region altindadir.
/// </summary>
public sealed partial class SystemSeeder : ISystemSeeder
{
    #region CEKIRDEK | Orchestration (SeedAllAsync) | IAM (Roller/Kullanicilar/Tipler) | Temel Menuler | Varsayilan Yetkiler

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

        // 1b) KURUMSAL ŞEMA — 134 kurumsal tabloyu (yoksa) modelden üretilen betikle
        //     idempotent sağlar. Taze SQL Server veritabanında EnsureCreated zaten
        //     oluşturduğu için bu adım no-op olur.
        await EnsureModuleTablesSchemaAsync(ct);

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

        // 5b) KURUMSAL VERİ — referans veriler, iş rolleri, modül menüleri, dashboard
        //     widget'ları ve varsayılan onay akışları (yetkiler senkronlandıktan sonra).
        _logger.LogInformation("Seeding: enterprise data (reference, roles, menus, widgets, approvals)");
        await SeedReferenceAndOperationalDataAsync(ct);

        // 5c) ÇEYREKLİK DEMO VERİSİ — son 90 güne (3 ay) yayılmış, tüm durum (case)
        //     varyasyonlarını içeren hacimli operasyonel kayıtlar; grid/rapor/onay
        //     ekranları gerçekçi ve dolu görünür.
        _logger.LogInformation("Seeding: demo quarter data (high-volume, all-status operational records, last 90 days)");
        await EnsureDemoQuarterDataAsync(ct);

        // 5d) DEMO HACİM — ana iş tablolarına 90 güne yayılmış, birbirinden bağımsız çok
        //     kayıt (stok hareketleri, borç/alacak/ödeme/tahsilat, hakediş, saha raporu,
        //     puantaj, belge+versiyon, talep); böylece zaman-serisi sistem davranışı gözlenir.
        _logger.LogInformation("Seeding: demo volume data (independent multi-record series across modules)");
        await EnsureDemoVolumeAsync(ct);

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

        // 7) DOĞRULAMA — tüm tohumlama tamamlandıktan sonra tablo-bazlı kapsama özetini
        //    üretir; satırı olmayan tabloları açıkça raporlar. Salt-okunur.
        _logger.LogInformation("Seeding: verification (per-table coverage summary)");
        await VerifySeedCoverageAsync(ct);
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
        await EnsureMenuAsync(LocalizationKeys.Menus.Chat, null, "/chat", "chat", 3, PermissionCatalog.ChatUse, ct);
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

    #endregion

    #region 01 | SISTEM SEMASI (idempotent DDL, migration'siz)

    /// <summary>
    /// Tüm şema işlemleri için tek giriş noktası. SQL Server'da şemanın tamamını
    /// modelden oluşturur (EnsureCreated); PostgreSQL'de geçiş içermeyen sütun/tablo
    /// tamamlamalarını uygular. İlk şemadan sonra eklenen çapraz kesen tablolar
    /// (sohbet grupları/ekleri, kullanıcı bazlı ayarlar) HER İKİ sağlayıcıda da
    /// sağlanır; çünkü EnsureCreated, zaten var olan bir SQL Server veritabanına yeni
    /// tablolar eklemez. Her adım idempotenttir.
    /// </summary>
    private async Task EnsureSchemaAsync(CancellationToken ct)
    {
        if (_db.Database.IsSqlServer())
        {
            _logger.LogInformation("Seeding: ensuring SQL Server schema (EnsureCreated)");
            await _db.Database.EnsureCreatedAsync(ct);
        }
        else
        {
            _logger.LogInformation("Seeding: audit log schema (request/response columns)");
            await EnsureAuditSchemaAsync(ct);

            _logger.LogInformation("Seeding: direct user-permission table");
            await EnsureUserPermissionSchemaAsync(ct);

            _logger.LogInformation("Seeding: profile-image columns");
            await EnsureProfileImageSchemaAsync(ct);

            _logger.LogInformation("Seeding: chat message table");
            await EnsureChatSchemaAsync(ct);
        }

        _logger.LogInformation("Seeding: chat group tables + message GroupId column");
        await EnsureChatGroupSchemaAsync(ct);

        _logger.LogInformation("Seeding: chat reply column + reactions table");
        await EnsureChatExtrasSchemaAsync(ct);

        _logger.LogInformation("Seeding: per-user settings table");
        await EnsureUserSettingsSchemaAsync(ct);
    }

    /// <summary>
    /// İstek/yanıt denetim sütunlarını idempotent şekilde ekler. Projenin geçiş
    /// geçmişi yoktur; bu nedenle bu, herhangi bir denetim eklemesi çalışmadan önce
    /// mevcut veritabanlarının yeni sütunları kazanmasını garanti eder. Yeni
    /// veritabanlarında güvenli ve işlemsizdir (no-op).
    /// </summary>
    private async Task EnsureAuditSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "QueryString" character varying(2000);
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "Source" character varying(10);
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "RequestBody" text;
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "ResponseBody" text;
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure AuditLogs request/response columns; they may already exist or the table is not yet created.");
        }
    }

    /// <summary>
    /// Doğrudan, kullanıcı bazlı yetki atamalarını (Kullanıcı Erişimi ekranından
    /// yönetilir) destekleyen <c>UserPermissions</c> tablosunu idempotent şekilde
    /// oluşturur. Denetim sütunları için kullanılan geçiş içermeyen yaklaşımı yansıtır.
    /// </summary>
    private async Task EnsureUserPermissionSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS "UserPermissions" (
                "UserId" uuid NOT NULL,
                "PermissionCode" character varying(150) NOT NULL,
                CONSTRAINT "PK_UserPermissions" PRIMARY KEY ("UserId", "PermissionCode"),
                CONSTRAINT "FK_UserPermissions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_UserPermissions_Permissions_PermissionCode" FOREIGN KEY ("PermissionCode") REFERENCES "Permissions" ("Code") ON DELETE RESTRICT
            );
            CREATE INDEX IF NOT EXISTS "IX_UserPermissions_PermissionCode" ON "UserPermissions" ("PermissionCode");
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the UserPermissions table; it may already exist or a referenced table is not yet created.");
        }
    }

    /// <summary>
    /// İkili profil resmi sütunlarını <c>Users</c> tablosuna idempotent şekilde ekler.
    /// Zaten varsa güvenli ve işlemsizdir (geçiş içermeyen konvansiyon).
    /// </summary>
    private async Task EnsureProfileImageSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ProfileImage" bytea;
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ProfileImageContentType" character varying(100);
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the Users profile-image columns; they may already exist.");
        }
    }

    /// <summary>
    /// Doğrudan mesajlaşma özelliğini destekleyen <c>ChatMessages</c> tablosunu
    /// idempotent şekilde oluşturur. Geçiş içermeyen DDL yaklaşımını yansıtır.
    /// </summary>
    private async Task EnsureChatSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS "ChatMessages" (
                "Id" uuid NOT NULL,
                "SenderId" uuid NOT NULL,
                "RecipientId" uuid NOT NULL,
                "Text" character varying(4000) NOT NULL,
                "IsRead" boolean NOT NULL DEFAULT FALSE,
                "ReadAt" timestamp with time zone,
                "CreatedAt" timestamp with time zone NOT NULL,
                "CreatedBy" uuid,
                "UpdatedAt" timestamp with time zone,
                "UpdatedBy" uuid,
                "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                "DeletedAt" timestamp with time zone,
                "DeletedBy" uuid,
                CONSTRAINT "PK_ChatMessages" PRIMARY KEY ("Id"),
                CONSTRAINT "FK_ChatMessages_Users_SenderId" FOREIGN KEY ("SenderId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_ChatMessages_Users_RecipientId" FOREIGN KEY ("RecipientId") REFERENCES "Users" ("Id") ON DELETE CASCADE
            );
            CREATE INDEX IF NOT EXISTS "IX_ChatMessages_SenderId_RecipientId" ON "ChatMessages" ("SenderId", "RecipientId");
            CREATE INDEX IF NOT EXISTS "IX_ChatMessages_RecipientId_IsRead" ON "ChatMessages" ("RecipientId", "IsRead");
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the ChatMessages table; it may already exist or a referenced table is not yet created.");
        }
    }

    /// <summary>
    /// <c>ChatGroups</c> / <c>ChatGroupMembers</c> tablolarını (<c>IsAdmin</c> sütunu
    /// dahil) ve <c>ChatMessages.GroupId</c> sütununu idempotent şekilde oluşturur;
    /// ayrıca <c>ChatMessages.RecipientId</c> sütununu NULL kabul edecek şekilde
    /// gevşetir (grup mesajlarının tek bir alıcısı yoktur). Sağlayıcıya özgü ancak idempotenttir.
    /// </summary>
    private async Task EnsureChatGroupSchemaAsync(CancellationToken ct)
    {
        var sql = _db.Database.IsSqlServer()
            ? """
              IF OBJECT_ID(N'[ChatGroups]', N'U') IS NULL
              CREATE TABLE [ChatGroups] (
                  [Id] uniqueidentifier NOT NULL,
                  [Name] nvarchar(150) NOT NULL,
                  [OwnerId] uniqueidentifier NOT NULL,
                  [CreatedAt] datetime2 NOT NULL,
                  [CreatedBy] uniqueidentifier NULL,
                  [UpdatedAt] datetime2 NULL,
                  [UpdatedBy] uniqueidentifier NULL,
                  [IsDeleted] bit NOT NULL CONSTRAINT [DF_ChatGroups_IsDeleted] DEFAULT(0),
                  [DeletedAt] datetime2 NULL,
                  [DeletedBy] uniqueidentifier NULL,
                  CONSTRAINT [PK_ChatGroups] PRIMARY KEY ([Id])
              );
              IF OBJECT_ID(N'[ChatGroupMembers]', N'U') IS NULL
              CREATE TABLE [ChatGroupMembers] (
                  [Id] uniqueidentifier NOT NULL,
                  [GroupId] uniqueidentifier NOT NULL,
                  [UserId] uniqueidentifier NOT NULL,
                  [Status] int NOT NULL,
                  [IsOwner] bit NOT NULL CONSTRAINT [DF_ChatGroupMembers_IsOwner] DEFAULT(0),
                  [InvitedById] uniqueidentifier NULL,
                  [CreatedAt] datetime2 NOT NULL,
                  [CreatedBy] uniqueidentifier NULL,
                  [UpdatedAt] datetime2 NULL,
                  [UpdatedBy] uniqueidentifier NULL,
                  [IsDeleted] bit NOT NULL CONSTRAINT [DF_ChatGroupMembers_IsDeleted] DEFAULT(0),
                  [DeletedAt] datetime2 NULL,
                  [DeletedBy] uniqueidentifier NULL,
                  CONSTRAINT [PK_ChatGroupMembers] PRIMARY KEY ([Id]),
                  CONSTRAINT [FK_ChatGroupMembers_ChatGroups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [ChatGroups]([Id]) ON DELETE CASCADE
              );
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatGroupMembers_GroupId_UserId')
              CREATE UNIQUE INDEX [IX_ChatGroupMembers_GroupId_UserId] ON [ChatGroupMembers]([GroupId],[UserId]);
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatGroupMembers_UserId_Status')
              CREATE INDEX [IX_ChatGroupMembers_UserId_Status] ON [ChatGroupMembers]([UserId],[Status]);
              IF COL_LENGTH('ChatGroupMembers','IsAdmin') IS NULL ALTER TABLE [ChatGroupMembers] ADD [IsAdmin] bit NOT NULL CONSTRAINT [DF_ChatGroupMembers_IsAdmin] DEFAULT(0);
              IF COL_LENGTH('ChatMessages','GroupId') IS NULL ALTER TABLE [ChatMessages] ADD [GroupId] uniqueidentifier NULL;
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessages_GroupId')
              CREATE INDEX [IX_ChatMessages_GroupId] ON [ChatMessages]([GroupId]);
              IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ChatMessages') AND name = 'RecipientId' AND is_nullable = 0)
              BEGIN
                  IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessages_SenderId_RecipientId') DROP INDEX [IX_ChatMessages_SenderId_RecipientId] ON [ChatMessages];
                  IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessages_RecipientId_IsRead') DROP INDEX [IX_ChatMessages_RecipientId_IsRead] ON [ChatMessages];
                  ALTER TABLE [ChatMessages] ALTER COLUMN [RecipientId] uniqueidentifier NULL;
                  CREATE INDEX [IX_ChatMessages_SenderId_RecipientId] ON [ChatMessages]([SenderId],[RecipientId]);
                  CREATE INDEX [IX_ChatMessages_RecipientId_IsRead] ON [ChatMessages]([RecipientId],[IsRead]);
              END
              """
            : """
              CREATE TABLE IF NOT EXISTS "ChatGroups" (
                  "Id" uuid NOT NULL,
                  "Name" character varying(150) NOT NULL,
                  "OwnerId" uuid NOT NULL,
                  "CreatedAt" timestamp with time zone NOT NULL,
                  "CreatedBy" uuid,
                  "UpdatedAt" timestamp with time zone,
                  "UpdatedBy" uuid,
                  "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                  "DeletedAt" timestamp with time zone,
                  "DeletedBy" uuid,
                  CONSTRAINT "PK_ChatGroups" PRIMARY KEY ("Id")
              );
              CREATE TABLE IF NOT EXISTS "ChatGroupMembers" (
                  "Id" uuid NOT NULL,
                  "GroupId" uuid NOT NULL,
                  "UserId" uuid NOT NULL,
                  "Status" integer NOT NULL,
                  "IsOwner" boolean NOT NULL DEFAULT FALSE,
                  "InvitedById" uuid,
                  "CreatedAt" timestamp with time zone NOT NULL,
                  "CreatedBy" uuid,
                  "UpdatedAt" timestamp with time zone,
                  "UpdatedBy" uuid,
                  "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                  "DeletedAt" timestamp with time zone,
                  "DeletedBy" uuid,
                  CONSTRAINT "PK_ChatGroupMembers" PRIMARY KEY ("Id"),
                  CONSTRAINT "FK_ChatGroupMembers_ChatGroups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "ChatGroups" ("Id") ON DELETE CASCADE
              );
              CREATE UNIQUE INDEX IF NOT EXISTS "IX_ChatGroupMembers_GroupId_UserId" ON "ChatGroupMembers" ("GroupId","UserId");
              CREATE INDEX IF NOT EXISTS "IX_ChatGroupMembers_UserId_Status" ON "ChatGroupMembers" ("UserId","Status");
              ALTER TABLE "ChatGroupMembers" ADD COLUMN IF NOT EXISTS "IsAdmin" boolean NOT NULL DEFAULT FALSE;
              ALTER TABLE "ChatMessages" ADD COLUMN IF NOT EXISTS "GroupId" uuid;
              CREATE INDEX IF NOT EXISTS "IX_ChatMessages_GroupId" ON "ChatMessages" ("GroupId");
              ALTER TABLE "ChatMessages" ALTER COLUMN "RecipientId" DROP NOT NULL;
              """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the chat group schema; parts may already exist.");
        }
    }

    /// <summary>
    /// <c>ChatMessages.ReplyToMessageId</c> sütununu idempotent şekilde ekler ve
    /// <c>ChatMessageReactions</c> tablosunu (emoji tepkileri) oluşturur. Her iki
    /// sağlayıcıda idempotenttir.
    /// </summary>
    private async Task EnsureChatExtrasSchemaAsync(CancellationToken ct)
    {
        var sql = _db.Database.IsSqlServer()
            ? """
              IF COL_LENGTH('ChatMessages','ReplyToMessageId') IS NULL ALTER TABLE [ChatMessages] ADD [ReplyToMessageId] uniqueidentifier NULL;
              IF OBJECT_ID(N'[ChatMessageReactions]', N'U') IS NULL
              CREATE TABLE [ChatMessageReactions] (
                  [Id] uniqueidentifier NOT NULL,
                  [MessageId] uniqueidentifier NOT NULL,
                  [UserId] uniqueidentifier NOT NULL,
                  [Emoji] nvarchar(16) NOT NULL,
                  [CreatedAt] datetime2 NOT NULL,
                  [CreatedBy] uniqueidentifier NULL,
                  [UpdatedAt] datetime2 NULL,
                  [UpdatedBy] uniqueidentifier NULL,
                  [IsDeleted] bit NOT NULL CONSTRAINT [DF_ChatMessageReactions_IsDeleted] DEFAULT(0),
                  [DeletedAt] datetime2 NULL,
                  [DeletedBy] uniqueidentifier NULL,
                  CONSTRAINT [PK_ChatMessageReactions] PRIMARY KEY ([Id]),
                  CONSTRAINT [FK_ChatMessageReactions_ChatMessages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [ChatMessages]([Id]) ON DELETE CASCADE
              );
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessageReactions_MessageId_UserId')
              CREATE UNIQUE INDEX [IX_ChatMessageReactions_MessageId_UserId] ON [ChatMessageReactions]([MessageId],[UserId]);
              """
            : """
              ALTER TABLE "ChatMessages" ADD COLUMN IF NOT EXISTS "ReplyToMessageId" uuid;
              CREATE TABLE IF NOT EXISTS "ChatMessageReactions" (
                  "Id" uuid NOT NULL,
                  "MessageId" uuid NOT NULL,
                  "UserId" uuid NOT NULL,
                  "Emoji" character varying(16) NOT NULL,
                  "CreatedAt" timestamp with time zone NOT NULL,
                  "CreatedBy" uuid,
                  "UpdatedAt" timestamp with time zone,
                  "UpdatedBy" uuid,
                  "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                  "DeletedAt" timestamp with time zone,
                  "DeletedBy" uuid,
                  CONSTRAINT "PK_ChatMessageReactions" PRIMARY KEY ("Id"),
                  CONSTRAINT "FK_ChatMessageReactions_ChatMessages_MessageId" FOREIGN KEY ("MessageId") REFERENCES "ChatMessages" ("Id") ON DELETE CASCADE
              );
              CREATE UNIQUE INDEX IF NOT EXISTS "IX_ChatMessageReactions_MessageId_UserId" ON "ChatMessageReactions" ("MessageId","UserId");
              """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the chat extras schema; parts may already exist.");
        }
    }

    /// <summary>
    /// Kullanıcı bazlı tercihleri (bildirim sesi, tema, okundu bilgileri, ...) destekleyen
    /// <c>UserSettings</c> tablosunu idempotent şekilde oluşturur. Her iki sağlayıcıda
    /// idempotenttir (geçiş içermeyen konvansiyon).
    /// </summary>
    private async Task EnsureUserSettingsSchemaAsync(CancellationToken ct)
    {
        var sql = _db.Database.IsSqlServer()
            ? """
              IF OBJECT_ID(N'[UserSettings]', N'U') IS NULL
              CREATE TABLE [UserSettings] (
                  [UserId] uniqueidentifier NOT NULL,
                  [NotificationSound] bit NOT NULL CONSTRAINT [DF_UserSettings_NotificationSound] DEFAULT(1),
                  [CallSound] bit NOT NULL CONSTRAINT [DF_UserSettings_CallSound] DEFAULT(1),
                  [DesktopNotifications] bit NOT NULL CONSTRAINT [DF_UserSettings_DesktopNotifications] DEFAULT(1),
                  [ReadReceipts] bit NOT NULL CONSTRAINT [DF_UserSettings_ReadReceipts] DEFAULT(1),
                  [Theme] nvarchar(20) NOT NULL CONSTRAINT [DF_UserSettings_Theme] DEFAULT(N'system'),
                  [UpdatedAt] datetime2 NULL,
                  CONSTRAINT [PK_UserSettings] PRIMARY KEY ([UserId]),
                  CONSTRAINT [FK_UserSettings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE
              );
              """
            : """
              CREATE TABLE IF NOT EXISTS "UserSettings" (
                  "UserId" uuid NOT NULL,
                  "NotificationSound" boolean NOT NULL DEFAULT TRUE,
                  "CallSound" boolean NOT NULL DEFAULT TRUE,
                  "DesktopNotifications" boolean NOT NULL DEFAULT TRUE,
                  "ReadReceipts" boolean NOT NULL DEFAULT TRUE,
                  "Theme" character varying(20) NOT NULL DEFAULT 'system',
                  "UpdatedAt" timestamp with time zone,
                  CONSTRAINT "PK_UserSettings" PRIMARY KEY ("UserId"),
                  CONSTRAINT "FK_UserSettings_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
              );
              """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the UserSettings table; it may already exist or a referenced table is not yet created.");
        }
    }

    #endregion

    #region 02 | LOOKUP & MASTER DATA | IS ROLLERI | MODUL MENU KOKLERI | ONAY TANIMLARI | DASHBOARD

    /// <summary>
    /// Para birimi tohum kayıtları (Seed Data sayfası). Görünen ad, sistemdeki diğer
    /// yerleşik kayıtlarla (DashboardWidgets, Roles, Menus, ApprovalDefinitions) tutarlı
    /// olacak şekilde gömülü metin yerine yerelleştirme anahtarı (<c>Currencies.{Code}.Name</c>)
    /// olarak saklanır ve gösterimde localizer ile çözülür.
    /// </summary>
    private static readonly (string Code, string Symbol)[] SeedCurrencies =
    [
        ("TRY", "₺"),
        ("USD", "$"),
        ("EUR", "€"),
    ];

    /// <summary>
    /// Ölçü birimi tohum kayıtları (Seed Data sayfası). Görünen ad, gömülü metin yerine
    /// yerelleştirme anahtarı (<c>Units.{Code}.Name</c>) olarak saklanır.
    /// </summary>
    private static readonly (string Code, string Symbol)[] SeedUnits =
    [
        ("Piece", "pcs"),
        ("Meter", "m"),
        ("Kilogram", "kg"),
        ("Ton", "t"),
        ("Liter", "L"),
        ("Hour", "h"),
        ("Day", "d"),
        ("Roll", "rl"),
        ("Package", "pkg"),
    ];

    /// <summary>
    /// Seed Data sayfasındaki iş rolleri ve her birinin yetki kümesi. Yetkiler
    /// merkezi <see cref="PermissionCatalog"/>'tan modül CRUD genişletmesiyle üretilir.
    /// </summary>
    private static readonly (string Name, string[] Modules, string[] Extra)[] BusinessRoles =
    [
        ("ProjectManager",
            ["Projects", "Requests", "Operations", "FieldOperations", "Documents", "Reporting"],
            ["Inventory.Read", "Procurement.Read", "Workflow.Approve", "Workflow.Reject", "Workflow.Return", "Reporting.Export"]),
        ("WarehouseManager",
            ["Inventory", "Catalog"],
            ["Inventory.Approve", "Inventory.Transfer", "Inventory.Count", "Inventory.Reverse", "Requests.Read"]),
        ("PurchaseManager",
            ["Procurement", "Requests"],
            ["Procurement.Approve", "Inventory.Read", "Catalog.Read", "Workflow.Approve", "Workflow.Reject"]),
        ("FinanceManager",
            ["Finance", "Budget", "Contracts", "ProgressPayments"],
            ["Workflow.Approve", "Workflow.Reject", "Reporting.Export", "Reporting.Read"]),
        ("HRManager",
            ["HR", "Organization"],
            ["Workflow.Approve", "Workflow.Reject", "Reporting.Read"]),
        ("SiteSupervisor",
            ["FieldOperations", "Operations"],
            ["Projects.Read", "Inventory.Read", "Documents.Upload", "Documents.Download"]),
    ];

    /// <summary>Kurumsal veri tohumlamasını sırayla çalıştırır (şema ayrı, daha erken sağlanır).</summary>
    private async Task SeedReferenceAndOperationalDataAsync(CancellationToken ct)
    {

        _logger.LogInformation("Seeding: enterprise reference data (currencies, units)");
        await EnsureModuleReferenceDataAsync(ct);

        _logger.LogInformation("Seeding: business roles + permission grants");
        await EnsureBusinessRolesAsync(ct);

        _logger.LogInformation("Seeding: enterprise module menus");
        await EnsureModuleMenusAsync(ct);

        _logger.LogInformation("Seeding: per-entity module menus");
        await EnsureEntityMenusAsync(ct);

        _logger.LogInformation("Seeding: per-report module menus");
        await EnsureReportMenusAsync(ct);

        _logger.LogInformation("Seeding: per-process module menus");
        await EnsureProcessMenusAsync(ct);

        _logger.LogInformation("Seeding: dashboard widgets");
        await EnsureDashboardWidgetsAsync(ct);

        _logger.LogInformation("Seeding: default approval definitions");
        await EnsureDefaultApprovalDefinitionsAsync(ct);

        _logger.LogInformation("Seeding: sample business data (header + line demo records)");
        await EnsureSampleBusinessDataAsync(ct);

        _logger.LogInformation("Seeding: full sample data across every remaining table");
        await EnsureFullSampleDataAsync(ct);
    }

    /// <summary>
    /// Kurumsal tabloları idempotent şekilde sağlar. SQL Server'da taze bir veritabanı
    /// EnsureCreated ile zaten tüm tabloları oluşturur; bu adım yalnızca tablolar henüz
    /// yoksa (örn. yalnızca IAM tablolarını içeren mevcut bir veritabanı) modelden
    /// üretilen oluşturma betiğini ifade ifade çalıştırır ve var olan nesneleri atlar.
    /// </summary>
    private async Task EnsureModuleTablesSchemaAsync(CancellationToken ct)
    {
        if (await TableExistsAsync("Companies", ct))
        {
            return;
        }

        string script = _db.Database.GenerateCreateScript();
        var statements = SplitSqlStatements(script);

        var created = 0;
        foreach (var statement in statements)
        {
            try
            {
                await _db.Database.ExecuteSqlRawAsync(statement, ct);
                created++;
            }
            catch (Exception ex)
            {
                // Var olan nesneler (IAM/Chat tabloları) yeniden oluşturulamaz; bu beklenen bir durumdur.
                _logger.LogDebug(ex, "Enterprise schema statement skipped (object may already exist).");
            }
        }

        _logger.LogInformation("Enterprise schema: {Count} statement(s) applied.", created);
    }

    private async Task<bool> TableExistsAsync(string tableName, CancellationToken ct)
    {
        try
        {
            var count = await _db.Database
                .SqlQueryRaw<int>(
                    "SELECT COUNT(*) AS \"Value\" FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0}",
                    tableName)
                .FirstOrDefaultAsync(ct);
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not determine whether table {Table} exists; assuming it does.", tableName);
            return true;
        }
    }

    private IEnumerable<string> SplitSqlStatements(string script)
    {
        if (_db.Database.IsSqlServer())
        {
            return Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0);
        }

        return script.Split(';')
            .Select(s => s.Trim())
            .Where(s => s.Length > 0);
    }

    private async Task EnsureModuleReferenceDataAsync(CancellationToken ct)
    {
        foreach (var (code, symbol) in SeedCurrencies)
        {
            if (!await _db.Currencies.IgnoreQueryFilters().AnyAsync(c => c.Code == code, ct))
            {
                _db.Currencies.Add(new Currency { Id = Guid.NewGuid(), Code = code, Name = $"Currencies.{code}.Name", Symbol = symbol, IsActive = true });
            }
        }

        foreach (var (code, symbol) in SeedUnits)
        {
            if (!await _db.UnitsOfMeasure.IgnoreQueryFilters().AnyAsync(u => u.Code == code, ct))
            {
                _db.UnitsOfMeasure.Add(new UnitOfMeasure { Id = Guid.NewGuid(), Code = code, Name = $"Units.{code}.Name", Symbol = symbol, IsActive = true });
            }
        }

        await _db.SaveChangesAsync(ct);

        // Tüm iş tipleri / süreç tanımları (proje türü/durumu, iş emri türü, talep türü,
        // stok belge türü, malzeme kategori ağacı) eksiksiz tohumlanır. Böylece sistemde
        // var olabilecek tüm iş tipleri ve süreçler için seçilebilir lookup verisi hazır olur.
        await EnsureBusinessTypeCatalogAsync(ct);
    }

    /// <summary>
    /// Sistemde tanımlı olabilecek tüm iş tiplerini ve süreç sınıflandırmalarını
    /// (proje türü/durumu, iş emri türü, talep türü, stok belge türü, malzeme kategorileri)
    /// idempotent şekilde tohumlar. Mevcut kayıtlar koda göre korunur, yenileri eklenir.
    /// </summary>
    private async Task EnsureBusinessTypeCatalogAsync(CancellationToken ct)
    {
        foreach (var (code, name) in SeedProjectTypes)
        {
            await GetOrAddAsync(_db.ProjectTypes, t => t.Code == code,
                () => new ProjectType { Id = Guid.NewGuid(), Code = code, Name = name, IsActive = true }, ct);
        }

        foreach (var (code, name, order, closed) in SeedProjectStatuses)
        {
            await GetOrAddAsync(_db.ProjectStatuses, s => s.Code == code,
                () => new ProjectStatus { Id = Guid.NewGuid(), Code = code, Name = name, DisplayOrder = order, IsClosedState = closed, IsActive = true }, ct);
        }

        foreach (var (code, name) in SeedWorkOrderTypes)
        {
            await GetOrAddAsync(_db.WorkOrderTypes, t => t.Code == code,
                () => new WorkOrderType { Id = Guid.NewGuid(), Code = code, Name = name, IsActive = true }, ct);
        }

        foreach (var (code, name, category) in SeedRequestTypes)
        {
            await GetOrAddAsync(_db.RequestTypes, t => t.Code == code,
                () => new RequestType { Id = Guid.NewGuid(), Code = code, Name = name, Category = category, IsActive = true }, ct);
        }

        foreach (var (code, name, direction) in SeedStockDocumentTypes)
        {
            await GetOrAddAsync(_db.StockDocumentTypes, t => t.Code == code,
                () => new StockDocumentType { Id = Guid.NewGuid(), Code = code, Name = name, Direction = direction, IsActive = true }, ct);
        }

        foreach (var (code, name) in SeedMaterialCategories)
        {
            await GetOrAddAsync(_db.MaterialCategories, c => c.Code == code,
                () => new MaterialCategory { Id = Guid.NewGuid(), Code = code, Name = name, IsActive = true }, ct);
        }

        _logger.LogInformation(
            "Business type catalog seeded: {ProjectTypes} project types, {ProjectStatuses} statuses, {WorkOrderTypes} work-order types, {RequestTypes} request types, {StockDocTypes} stock document types, {Categories} material categories.",
            SeedProjectTypes.Length, SeedProjectStatuses.Length, SeedWorkOrderTypes.Length,
            SeedRequestTypes.Length, SeedStockDocumentTypes.Length, SeedMaterialCategories.Length);
    }

    /// <summary>Tüm proje türleri (kod, ad).</summary>
    private static readonly (string Code, string Name)[] SeedProjectTypes =
    [
        ("CONSTR", "İnşaat"),
        ("INFRA", "Altyapı"),
        ("RENO", "Renovasyon / Tadilat"),
        ("MAINT", "Bakım-Onarım"),
        ("EPC", "EPC (Mühendislik-Tedarik-İnşaat)"),
        ("CONSULT", "Müşavirlik / Danışmanlık"),
        ("ELECTRICAL", "Elektrik Tesisat"),
        ("MECHANICAL", "Mekanik Tesisat"),
        ("ENERGY", "Enerji / Yenilenebilir"),
    ];

    /// <summary>Tüm proje durumları (kod, ad, sıra, kapalı durum mu).</summary>
    private static readonly (string Code, string Name, int Order, bool Closed)[] SeedProjectStatuses =
    [
        ("PLANNING", "Planlama", 1, false),
        ("ACTIVE", "Aktif", 2, false),
        ("ONHOLD", "Beklemede", 3, false),
        ("SUSPENDED", "Askıya Alındı", 4, false),
        ("COMPLETED", "Tamamlandı", 5, true),
        ("CANCELLED", "İptal Edildi", 6, true),
        ("CLOSED", "Kapatıldı", 7, true),
    ];

    /// <summary>Tüm iş emri türleri (kod, ad).</summary>
    private static readonly (string Code, string Name)[] SeedWorkOrderTypes =
    [
        ("WOT-001", "Saha İşi"),
        ("WOT-MAINT", "Bakım"),
        ("WOT-INSTALL", "Montaj / Kurulum"),
        ("WOT-INSPECT", "Muayene / Kontrol"),
        ("WOT-REPAIR", "Onarım"),
        ("WOT-EMERGENCY", "Acil Müdahale"),
        ("WOT-CIVIL", "İnşaat İmalatı"),
        ("WOT-ELECTRIC", "Elektrik İşi"),
        ("WOT-MECHANIC", "Mekanik İş"),
    ];

    /// <summary>Tüm talep türleri (kod, ad, kategori).</summary>
    private static readonly (string Code, string Name, string Category)[] SeedRequestTypes =
    [
        ("RQT-001", "Malzeme Talebi", "Material"),
        ("RQT-PURCHASE", "Satınalma Talebi", "Material"),
        ("RQT-SERVICE", "Hizmet Talebi", "Service"),
        ("RQT-EQUIP", "Ekipman Talebi", "Equipment"),
        ("RQT-PERSONNEL", "Personel Talebi", "Personnel"),
        ("RQT-MAINT", "Bakım Talebi", "Service"),
        ("RQT-TRANSPORT", "Nakliye Talebi", "Service"),
        ("RQT-OTHER", "Diğer Talepler", "Other"),
    ];

    /// <summary>Tüm stok belge türleri (kod, ad, yön).</summary>
    private static readonly (string Code, string Name, string Direction)[] SeedStockDocumentTypes =
    [
        ("SDT-IN", "Mal Girişi", "In"),
        ("SDT-OUT", "Sarf Çıkışı", "Out"),
        ("SDT-TRF", "Depo Transferi", "Transfer"),
        ("SDT-ADJ", "Stok Düzeltme", "Adjustment"),
        ("SDT-RET", "İade Girişi", "In"),
        ("SDT-SCRAP", "Hurda / Fire Çıkışı", "Out"),
        ("SDT-PROD", "Üretim Girişi", "In"),
        ("SDT-CONS", "Sahaya Sarf", "Out"),
    ];

    /// <summary>Tüm malzeme kategorileri (kod, ad) — kök seviye sınıflandırma.</summary>
    private static readonly (string Code, string Name)[] SeedMaterialCategories =
    [
        ("CAT-001", "Genel Malzeme"),
        ("CAT-CEMENT", "Çimento ve Bağlayıcılar"),
        ("CAT-STEEL", "Demir-Çelik"),
        ("CAT-AGG", "Agrega ve Kum"),
        ("CAT-ELEC", "Elektrik Malzemeleri"),
        ("CAT-PLUMB", "Tesisat / Sıhhi Tesisat"),
        ("CAT-PAINT", "Boya ve Kaplama"),
        ("CAT-TOOL", "El Aletleri ve Ekipman"),
        ("CAT-SAFETY", "İş Güvenliği Malzemeleri"),
        ("CAT-CONSUM", "Sarf Malzemeleri"),
    ];

    private async Task EnsureBusinessRolesAsync(CancellationToken ct)
    {
        // Admin = tüm yetkiler (SuperAdmin'den farklı; yetki tabanlı, yönetilebilir).
        await EnsureRoleWithPermissionsAsync("Admin", [.. PermissionCatalog.All.Select(p => p.Code)], ct);

        foreach (var (name, modules, extra) in BusinessRoles)
        {
            var codes = new List<string>();
            foreach (var module in modules)
            {
                codes.Add($"{module}.{PermissionActions.Read}");
                codes.Add($"{module}.{PermissionActions.ReadAll}");
                codes.Add($"{module}.{PermissionActions.Create}");
                codes.Add($"{module}.{PermissionActions.Update}");
                codes.Add($"{module}.{PermissionActions.Delete}");
            }
            codes.AddRange(extra);
            codes.Add(PermissionCatalog.DashboardRead);

            await EnsureRoleWithPermissionsAsync(name, codes, ct);
        }

        // Her iş rolü için giriş yapılabilir bir demo kullanıcısı sağla; böylece
        // roller yalnızca tanımlı değil, uçtan uca denenebilir olur.
        await EnsureBusinessRoleDemoUsersAsync(ct);
    }

    /// <summary>İş rolü adı → demo kullanıcı bilgileri (kullanıcı adı, e-posta, ad, soyad, parola).</summary>
    private static readonly (string Role, string UserName, string Email, string FirstName, string LastName, string Password)[] BusinessRoleDemoUsers =
    [
        ("ProjectManager",   "project.manager",   "project.manager@energy.local",   "Burak", "Aslan",   "Project123!"),
        ("WarehouseManager", "warehouse.manager", "warehouse.manager@energy.local", "Cem",   "Korkmaz", "Warehouse123!"),
        ("PurchaseManager",  "purchase.manager",  "purchase.manager@energy.local",  "Gizem", "Arslan",  "Purchase123!"),
        ("FinanceManager",   "finance.manager",   "finance.manager@energy.local",   "Derya", "Polat",   "Finance123!"),
        ("HRManager",        "hr.manager",        "hr.manager@energy.local",        "Ece",   "Doğan",   "HumanRes123!"),
        ("SiteSupervisor",   "site.supervisor",   "site.supervisor@energy.local",   "Onur",  "Şimşek",  "SiteSup123!"),
    ];

    /// <summary>
    /// İş rollerine birer demo kullanıcı bağlar. Kullanıcı e-postaya göre idempotent
    /// sağlanır ve rol ataması yalnızca yoksa eklenir; mevcut kayıtlar değiştirilmez.
    /// </summary>
    private async Task EnsureBusinessRoleDemoUsersAsync(CancellationToken ct)
    {
        var usersAdded = 0;
        foreach (var (roleName, userName, email, firstName, lastName, password) in BusinessRoleDemoUsers)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
            if (role is null)
            {
                continue;
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = _passwords.Hash(password),
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid(),
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync(ct);
                usersAdded++;
            }

            if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
            {
                _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                await _db.SaveChangesAsync(ct);
                _permissionResolver.InvalidateUser(user.Id);
            }
        }

        if (usersAdded > 0)
        {
            _logger.LogInformation("Business roles: {Count} demo user(s) ensured.", usersAdded);
        }
    }

    private async Task EnsureRoleWithPermissionsAsync(string roleName, IReadOnlyCollection<string> permissionCodes, CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
        if (role is null)
        {
            role = new Role { Id = Guid.NewGuid(), Name = roleName, Description = $"Roles.{roleName}.Description", IsSystem = false };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(ct);
        }

        var existing = (await _db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionCode)
                .ToListAsync(ct))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var validCodes = PermissionCatalog.AllCodes;
        var changed = false;
        foreach (var code in permissionCodes.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (existing.Contains(code) || !validCodes.Contains(code))
            {
                continue;
            }
            _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionCode = code });
            changed = true;
        }

        if (changed)
        {
            await _db.SaveChangesAsync(ct);
            await _permissionResolver.InvalidateRoleAsync(role.Id, ct);
        }
    }

    private async Task EnsureModuleMenusAsync(CancellationToken ct)
    {
        // Alan bazlı üst menüler + modül girdileri. Her modül kendi <Module>.ReadAll yetkisiyle korunur.
        var projects = await EnsureMenuAsync("Menus.ProjectsArea", null, null, "hierarchy", 20, null, ct);
        await EnsureMenuAsync("Menus.Projects", projects.Id, null, "box", 21, "Projects.ReadAll", ct);
        await EnsureMenuAsync("Menus.Operations", projects.Id, null, "preferences", 22, "Operations.ReadAll", ct);
        await EnsureMenuAsync("Menus.FieldOperations", projects.Id, null, "map", 23, "FieldOperations.ReadAll", ct);
        await EnsureMenuAsync("Menus.Contracts", projects.Id, null, "doc", 24, "Contracts.ReadAll", ct);
        await EnsureMenuAsync("Menus.ProgressPayments", projects.Id, null, "money", 25, "ProgressPayments.ReadAll", ct);

        var supply = await EnsureMenuAsync("Menus.SupplyArea", null, null, "cart", 30, null, ct);
        await EnsureMenuAsync("Menus.Catalog", supply.Id, null, "detailslayout", 31, "Catalog.ReadAll", ct);
        await EnsureMenuAsync("Menus.Inventory", supply.Id, null, "box", 32, "Inventory.ReadAll", ct);
        await EnsureMenuAsync("Menus.Requests", supply.Id, null, "newfolder", 33, "Requests.ReadAll", ct);
        await EnsureMenuAsync("Menus.Procurement", supply.Id, null, "cart", 34, "Procurement.ReadAll", ct);

        var finance = await EnsureMenuAsync("Menus.FinanceArea", null, null, "money", 40, null, ct);
        await EnsureMenuAsync("Menus.Finance", finance.Id, null, "money", 41, "Finance.ReadAll", ct);
        await EnsureMenuAsync("Menus.Budget", finance.Id, null, "chart", 42, "Budget.ReadAll", ct);

        var hr = await EnsureMenuAsync("Menus.HRArea", null, null, "group", 50, null, ct);
        await EnsureMenuAsync("Menus.Organization", hr.Id, null, "group", 51, "Organization.ReadAll", ct);
        await EnsureMenuAsync("Menus.HR", hr.Id, null, "clock", 52, "HR.ReadAll", ct);
        await EnsureMenuAsync("Menus.Assets", hr.Id, null, "car", 53, "Assets.ReadAll", ct);

        var common = await EnsureMenuAsync("Menus.CommonArea", null, null, "more", 60, null, ct);
        await EnsureMenuAsync("Menus.BusinessPartners", common.Id, null, "card", 61, "BusinessPartners.ReadAll", ct);
        await EnsureMenuAsync("Menus.Documents", common.Id, null, "doc", 62, "Documents.ReadAll", ct);
        await EnsureMenuAsync("Menus.Workflow", common.Id, null, "check", 63, "Workflow.ReadAll", ct);
        await EnsureMenuAsync("Menus.Notifications", common.Id, null, "bell", 64, "Notifications.ReadAll", ct);
        await EnsureMenuAsync("Menus.Reporting", common.Id, null, "chart", 65, "Reporting.ReadAll", ct);
        await EnsureMenuAsync("Menus.CoreData", common.Id, null, "preferences", 66, "Core.ReadAll", ct);
    }

    private async Task EnsureDashboardWidgetsAsync(CancellationToken ct)
    {
        (string Code, string Module, string Type, int Order, string? Perm)[] widgets =
        [
            ("LowStock", "Inventory", "Counter", 1, "Inventory.ReadAll"),
            ("PendingApprovals", "Workflow", "Counter", 2, "Workflow.ReadAll"),
            ("BudgetOverrun", "Budget", "Chart", 3, "Budget.ReadAll"),
            ("OrderDelivery", "Procurement", "Grid", 4, "Procurement.ReadAll"),
            ("WorkOrderProgress", "Operations", "Gauge", 5, "Operations.ReadAll"),
        ];

        foreach (var (code, module, type, order, perm) in widgets)
        {
            if (!await _db.DashboardWidgets.IgnoreQueryFilters().AnyAsync(w => w.Code == code, ct))
            {
                _db.DashboardWidgets.Add(new DashboardWidget
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = $"DashboardWidgets.{code}.Name",
                    Module = module,
                    WidgetType = type,
                    RequiredPermissionCode = perm,
                    DisplayOrder = order,
                    IsActive = true,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Tasarım dokümanındaki örnek satın alma ve masraf onay akışlarını idempotent
    /// olarak oluşturur (definition + yürürlükteki versiyon + adımlar + onaycılar).
    /// </summary>
    private async Task EnsureDefaultApprovalDefinitionsAsync(CancellationToken ct)
    {
        await EnsureApprovalDefinitionAsync(
            code: "PurchaseOrderApproval",
            relatedModule: "Procurement",
            relatedEntityType: "PurchaseOrder",
            steps:
            [
                (1, "ProjectManager", ApprovalMode.Sequential),
                (2, "FinanceManager", ApprovalMode.Sequential),
            ],
            ct);

        await EnsureApprovalDefinitionAsync(
            code: "ExpenseClaimApproval",
            relatedModule: "Organization",
            relatedEntityType: "ExpenseClaim",
            steps:
            [
                (1, "ProjectManager", ApprovalMode.ParallelAny),
                (2, "FinanceManager", ApprovalMode.Sequential),
            ],
            ct);
    }

    private async Task EnsureApprovalDefinitionAsync(
        string code,
        string relatedModule,
        string relatedEntityType,
        IReadOnlyList<(int StepNo, string RoleName, ApprovalMode Mode)> steps,
        CancellationToken ct)
    {
        if (await _db.ApprovalDefinitions.IgnoreQueryFilters().AnyAsync(d => d.Code == code, ct))
        {
            return;
        }

        var definition = new ApprovalDefinition
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = $"ApprovalDefinitions.{code}.Name",
            RelatedModule = relatedModule,
            RelatedEntityType = relatedEntityType,
            IsActive = true,
        };
        _db.ApprovalDefinitions.Add(definition);

        var version = new ApprovalDefinitionVersion
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionId = definition.Id,
            VersionNo = 1,
            EffectiveFrom = DateTime.UtcNow,
            IsActive = true,
        };
        _db.ApprovalDefinitionVersions.Add(version);

        foreach (var (stepNo, roleName, mode) in steps)
        {
            var step = new ApprovalStepDefinition
            {
                Id = Guid.NewGuid(),
                ApprovalDefinitionVersionId = version.Id,
                StepNo = stepNo,
                Name = $"ApprovalSteps.{code}.{stepNo}",
                ApprovalMode = mode,
                IsRequired = true,
            };
            _db.ApprovalStepDefinitions.Add(step);

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
            _db.ApprovalStepApprovers.Add(new ApprovalStepApprover
            {
                Id = Guid.NewGuid(),
                ApprovalStepDefinitionId = step.Id,
                ApproverType = ApproverType.Role,
                ApproverRoleId = role?.Id,
            });
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region 03 | MENULER | Entity (modul basina ekran menuleri)

    /// <summary>(Module, ParentMenuNameKey, Entity, Route, NameKey, Order)</summary>
    private static readonly (string Module, string ParentKey, string Entity, string Route, string NameKey, int Order)[] ModuleEntityMenus =
    [
        ("Core", "Menus.CoreData", "Company", "/core/companies", "Menus.Core.Company", 1),
        ("Core", "Menus.CoreData", "Branch", "/core/branches", "Menus.Core.Branch", 2),
        ("Core", "Menus.CoreData", "Department", "/core/departments", "Menus.Core.Department", 3),
        ("Core", "Menus.CoreData", "Currency", "/core/currencies", "Menus.Core.Currency", 4),
        ("Core", "Menus.CoreData", "ExchangeRate", "/core/exchange-rates", "Menus.Core.ExchangeRate", 5),
        ("Core", "Menus.CoreData", "UnitOfMeasure", "/core/units-of-measure", "Menus.Core.UnitOfMeasure", 6),
        ("Core", "Menus.CoreData", "UnitConversion", "/core/unit-conversions", "Menus.Core.UnitConversion", 7),
        ("Core", "Menus.CoreData", "SequenceDefinition", "/core/sequence-definitions", "Menus.Core.SequenceDefinition", 8),
        ("Core", "Menus.CoreData", "SystemSetting", "/core/system-settings", "Menus.Core.SystemSetting", 9),
        ("Core", "Menus.CoreData", "LocalizationResource", "/core/localization-resources", "Menus.Core.LocalizationResource", 10),
        ("Core", "Menus.CoreData", "AuditLog", "/core/audit-logs", "Menus.Core.AuditLog", 11),
        ("Organization", "Menus.Organization", "Employee", "/organization/employees", "Menus.Organization.Employee", 1),
        ("Organization", "Menus.Organization", "EmployeePosition", "/organization/employee-positions", "Menus.Organization.EmployeePosition", 2),
        ("Organization", "Menus.Organization", "EmployeeSkill", "/organization/employee-skills", "Menus.Organization.EmployeeSkill", 3),
        ("Organization", "Menus.Organization", "EmployeeSkillAssignment", "/organization/employee-skill-assignments", "Menus.Organization.EmployeeSkillAssignment", 4),
        ("Organization", "Menus.Organization", "LeaveRequest", "/organization/leave-requests", "Menus.Organization.LeaveRequest", 5),
        ("Organization", "Menus.Organization", "ExpenseClaim", "/organization/expense-claims", "Menus.Organization.ExpenseClaim", 6),
        ("Organization", "Menus.Organization", "ExpenseClaimLine", "/organization/expense-claim-lines", "Menus.Organization.ExpenseClaimLine", 7),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartner", "/business-partners/business-partners", "Menus.BusinessPartners.BusinessPartner", 1),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartnerContact", "/business-partners/business-partner-contacts", "Menus.BusinessPartners.BusinessPartnerContact", 2),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartnerAddress", "/business-partners/business-partner-addresses", "Menus.BusinessPartners.BusinessPartnerAddress", 3),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartnerBankAccount", "/business-partners/business-partner-bank-accounts", "Menus.BusinessPartners.BusinessPartnerBankAccount", 4),
        ("Projects", "Menus.Projects", "Project", "/projects/projects", "Menus.Projects.Project", 1),
        ("Projects", "Menus.Projects", "ProjectType", "/projects/project-types", "Menus.Projects.ProjectType", 2),
        ("Projects", "Menus.Projects", "ProjectStatus", "/projects/project-statuses", "Menus.Projects.ProjectStatus", 3),
        ("Projects", "Menus.Projects", "ProjectLocation", "/projects/project-locations", "Menus.Projects.ProjectLocation", 4),
        ("Projects", "Menus.Projects", "ProjectPhas", "/projects/project-phases", "Menus.Projects.ProjectPhas", 5),
        ("Projects", "Menus.Projects", "ProjectMember", "/projects/project-members", "Menus.Projects.ProjectMember", 6),
        ("Projects", "Menus.Projects", "ProjectNote", "/projects/project-notes", "Menus.Projects.ProjectNote", 7),
        ("Catalog", "Menus.Catalog", "Brand", "/catalog/brands", "Menus.Catalog.Brand", 1),
        ("Catalog", "Menus.Catalog", "MaterialCategory", "/catalog/material-categories", "Menus.Catalog.MaterialCategory", 2),
        ("Catalog", "Menus.Catalog", "MaterialAttributeDefinition", "/catalog/material-attribute-definitions", "Menus.Catalog.MaterialAttributeDefinition", 3),
        ("Catalog", "Menus.Catalog", "MaterialAttributeOption", "/catalog/material-attribute-options", "Menus.Catalog.MaterialAttributeOption", 4),
        ("Catalog", "Menus.Catalog", "MaterialCategoryAttribute", "/catalog/material-category-attributes", "Menus.Catalog.MaterialCategoryAttribute", 5),
        ("Catalog", "Menus.Catalog", "Material", "/catalog/materials", "Menus.Catalog.Material", 6),
        ("Catalog", "Menus.Catalog", "MaterialAttributeValue", "/catalog/material-attribute-values", "Menus.Catalog.MaterialAttributeValue", 7),
        ("Catalog", "Menus.Catalog", "MaterialUnitConversion", "/catalog/material-unit-conversions", "Menus.Catalog.MaterialUnitConversion", 8),
        ("Inventory", "Menus.Inventory", "Warehouse", "/inventory/warehouses", "Menus.Inventory.Warehouse", 1),
        ("Inventory", "Menus.Inventory", "WarehouseLocation", "/inventory/warehouse-locations", "Menus.Inventory.WarehouseLocation", 2),
        ("Inventory", "Menus.Inventory", "StockDocumentType", "/inventory/stock-document-types", "Menus.Inventory.StockDocumentType", 3),
        ("Inventory", "Menus.Inventory", "StockDocument", "/inventory/stock-documents", "Menus.Inventory.StockDocument", 4),
        ("Inventory", "Menus.Inventory", "StockDocumentLine", "/inventory/stock-document-lines", "Menus.Inventory.StockDocumentLine", 5),
        ("Inventory", "Menus.Inventory", "StockLot", "/inventory/stock-lots", "Menus.Inventory.StockLot", 6),
        ("Inventory", "Menus.Inventory", "StockIssueAllocation", "/inventory/stock-issue-allocations", "Menus.Inventory.StockIssueAllocation", 7),
        ("Inventory", "Menus.Inventory", "StockTransaction", "/inventory/stock-transactions", "Menus.Inventory.StockTransaction", 8),
        ("Inventory", "Menus.Inventory", "StockBalance", "/inventory/stock-balances", "Menus.Inventory.StockBalance", 9),
        ("Inventory", "Menus.Inventory", "StockReservation", "/inventory/stock-reservations", "Menus.Inventory.StockReservation", 10),
        ("Inventory", "Menus.Inventory", "StockCount", "/inventory/stock-counts", "Menus.Inventory.StockCount", 11),
        ("Inventory", "Menus.Inventory", "StockCountLine", "/inventory/stock-count-lines", "Menus.Inventory.StockCountLine", 12),
        ("Inventory", "Menus.Inventory", "WarehouseTransfer", "/inventory/warehouse-transfers", "Menus.Inventory.WarehouseTransfer", 13),
        ("Inventory", "Menus.Inventory", "WarehouseTransferLine", "/inventory/warehouse-transfer-lines", "Menus.Inventory.WarehouseTransferLine", 14),
        ("Requests", "Menus.Requests", "RequestType", "/requests/request-types", "Menus.Requests.RequestType", 1),
        ("Requests", "Menus.Requests", "Request", "/requests/requests", "Menus.Requests.Request", 2),
        ("Requests", "Menus.Requests", "RequestLine", "/requests/request-lines", "Menus.Requests.RequestLine", 3),
        ("Procurement", "Menus.Procurement", "SupplierQuote", "/procurement/supplier-quotes", "Menus.Procurement.SupplierQuote", 1),
        ("Procurement", "Menus.Procurement", "SupplierQuoteLine", "/procurement/supplier-quote-lines", "Menus.Procurement.SupplierQuoteLine", 2),
        ("Procurement", "Menus.Procurement", "PurchaseOrder", "/procurement/purchase-orders", "Menus.Procurement.PurchaseOrder", 3),
        ("Procurement", "Menus.Procurement", "PurchaseOrderLine", "/procurement/purchase-order-lines", "Menus.Procurement.PurchaseOrderLine", 4),
        ("Procurement", "Menus.Procurement", "PurchaseReceipt", "/procurement/purchase-receipts", "Menus.Procurement.PurchaseReceipt", 5),
        ("Procurement", "Menus.Procurement", "PurchaseReceiptLine", "/procurement/purchase-receipt-lines", "Menus.Procurement.PurchaseReceiptLine", 6),
        ("Procurement", "Menus.Procurement", "SupplierInvoice", "/procurement/supplier-invoices", "Menus.Procurement.SupplierInvoice", 7),
        ("Procurement", "Menus.Procurement", "SupplierInvoiceLine", "/procurement/supplier-invoice-lines", "Menus.Procurement.SupplierInvoiceLine", 8),
        ("Operations", "Menus.Operations", "WorkOrderType", "/operations/work-order-types", "Menus.Operations.WorkOrderType", 1),
        ("Operations", "Menus.Operations", "WorkOrder", "/operations/work-orders", "Menus.Operations.WorkOrder", 2),
        ("Operations", "Menus.Operations", "WorkOrderAssignment", "/operations/work-order-assignments", "Menus.Operations.WorkOrderAssignment", 3),
        ("Operations", "Menus.Operations", "WorkOrderMaterialPlan", "/operations/work-order-material-plans", "Menus.Operations.WorkOrderMaterialPlan", 4),
        ("Operations", "Menus.Operations", "WorkOrderMaterialUsage", "/operations/work-order-material-usages", "Menus.Operations.WorkOrderMaterialUsage", 5),
        ("Operations", "Menus.Operations", "WorkOrderChecklist", "/operations/work-order-checklists", "Menus.Operations.WorkOrderChecklist", 6),
        ("Operations", "Menus.Operations", "WorkOrderChecklistItem", "/operations/work-order-checklist-items", "Menus.Operations.WorkOrderChecklistItem", 7),
        ("Operations", "Menus.Operations", "WorkOrderStatusHistory", "/operations/work-order-status-histories", "Menus.Operations.WorkOrderStatusHistory", 8),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReport", "/field-operations/daily-site-reports", "Menus.FieldOperations.DailySiteReport", 1),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReportWorker", "/field-operations/daily-site-report-workers", "Menus.FieldOperations.DailySiteReportWorker", 2),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReportEquipment", "/field-operations/daily-site-report-equipments", "Menus.FieldOperations.DailySiteReportEquipment", 3),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReportMaterial", "/field-operations/daily-site-report-materials", "Menus.FieldOperations.DailySiteReportMaterial", 4),
        ("FieldOperations", "Menus.FieldOperations", "ProgressEntry", "/field-operations/progress-entries", "Menus.FieldOperations.ProgressEntry", 5),
        ("FieldOperations", "Menus.FieldOperations", "MeasurementSheet", "/field-operations/measurement-sheets", "Menus.FieldOperations.MeasurementSheet", 6),
        ("FieldOperations", "Menus.FieldOperations", "MeasurementSheetLine", "/field-operations/measurement-sheet-lines", "Menus.FieldOperations.MeasurementSheetLine", 7),
        ("HR", "Menus.HR", "Timesheet", "/h-r/timesheets", "Menus.HR.Timesheet", 1),
        ("HR", "Menus.HR", "TimesheetLine", "/h-r/timesheet-lines", "Menus.HR.TimesheetLine", 2),
        ("Assets", "Menus.Assets", "EquipmentAsset", "/assets/equipment-assets", "Menus.Assets.EquipmentAsset", 1),
        ("Assets", "Menus.Assets", "EquipmentAssignment", "/assets/equipment-assignments", "Menus.Assets.EquipmentAssignment", 2),
        ("Assets", "Menus.Assets", "EquipmentMaintenance", "/assets/equipment-maintenances", "Menus.Assets.EquipmentMaintenance", 3),
        ("Finance", "Menus.Finance", "FinancialAccount", "/finance/financial-accounts", "Menus.Finance.FinancialAccount", 1),
        ("Finance", "Menus.Finance", "CostCenter", "/finance/cost-centers", "Menus.Finance.CostCenter", 2),
        ("Finance", "Menus.Finance", "FinancialTransaction", "/finance/financial-transactions", "Menus.Finance.FinancialTransaction", 3),
        ("Finance", "Menus.Finance", "FinancialTransactionLine", "/finance/financial-transaction-lines", "Menus.Finance.FinancialTransactionLine", 4),
        ("Finance", "Menus.Finance", "Payable", "/finance/payables", "Menus.Finance.Payable", 5),
        ("Finance", "Menus.Finance", "Receivable", "/finance/receivables", "Menus.Finance.Receivable", 6),
        ("Finance", "Menus.Finance", "Payment", "/finance/payments", "Menus.Finance.Payment", 7),
        ("Finance", "Menus.Finance", "PaymentAllocation", "/finance/payment-allocations", "Menus.Finance.PaymentAllocation", 8),
        ("Finance", "Menus.Finance", "Collection", "/finance/collections", "Menus.Finance.Collection", 9),
        ("Finance", "Menus.Finance", "CollectionAllocation", "/finance/collection-allocations", "Menus.Finance.CollectionAllocation", 10),
        ("Budget", "Menus.Budget", "Budget", "/budget/budgets", "Menus.Budget.Budget", 1),
        ("Budget", "Menus.Budget", "BudgetLine", "/budget/budget-lines", "Menus.Budget.BudgetLine", 2),
        ("Contracts", "Menus.Contracts", "Contract", "/contracts/contracts", "Menus.Contracts.Contract", 1),
        ("Contracts", "Menus.Contracts", "ContractParty", "/contracts/contract-parties", "Menus.Contracts.ContractParty", 2),
        ("Contracts", "Menus.Contracts", "ContractLine", "/contracts/contract-lines", "Menus.Contracts.ContractLine", 3),
        ("Contracts", "Menus.Contracts", "ContractAmendment", "/contracts/contract-amendments", "Menus.Contracts.ContractAmendment", 4),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPayment", "/progress-payments/progress-payments", "Menus.ProgressPayments.ProgressPayment", 1),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPaymentLine", "/progress-payments/progress-payment-lines", "Menus.ProgressPayments.ProgressPaymentLine", 2),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPaymentDeduction", "/progress-payments/progress-payment-deductions", "Menus.ProgressPayments.ProgressPaymentDeduction", 3),
        ("Documents", "Menus.Documents", "DocumentFolder", "/documents/document-folders", "Menus.Documents.DocumentFolder", 1),
        ("Documents", "Menus.Documents", "Document", "/documents/documents", "Menus.Documents.Document", 2),
        ("Documents", "Menus.Documents", "DocumentVersion", "/documents/document-versions", "Menus.Documents.DocumentVersion", 3),
        ("Documents", "Menus.Documents", "DocumentRelation", "/documents/document-relations", "Menus.Documents.DocumentRelation", 4),
        ("Documents", "Menus.Documents", "DocumentPermission", "/documents/document-permissions", "Menus.Documents.DocumentPermission", 5),
        ("Workflow", "Menus.Workflow", "ApprovalDefinition", "/workflow/approval-definitions", "Menus.Workflow.ApprovalDefinition", 1),
        ("Workflow", "Menus.Workflow", "ApprovalDefinitionVersion", "/workflow/approval-definition-versions", "Menus.Workflow.ApprovalDefinitionVersion", 2),
        ("Workflow", "Menus.Workflow", "ApprovalStepDefinition", "/workflow/approval-step-definitions", "Menus.Workflow.ApprovalStepDefinition", 3),
        ("Workflow", "Menus.Workflow", "ApprovalStepApprover", "/workflow/approval-step-approvers", "Menus.Workflow.ApprovalStepApprover", 4),
        ("Workflow", "Menus.Workflow", "ApprovalCondition", "/workflow/approval-conditions", "Menus.Workflow.ApprovalCondition", 5),
        ("Workflow", "Menus.Workflow", "ApprovalRequest", "/workflow/approval-requests", "Menus.Workflow.ApprovalRequest", 6),
        ("Workflow", "Menus.Workflow", "ApprovalRequestStep", "/workflow/approval-request-steps", "Menus.Workflow.ApprovalRequestStep", 7),
        ("Workflow", "Menus.Workflow", "ApprovalRequestApprover", "/workflow/approval-request-approvers", "Menus.Workflow.ApprovalRequestApprover", 8),
        ("Workflow", "Menus.Workflow", "ApprovalAction", "/workflow/approval-actions", "Menus.Workflow.ApprovalAction", 9),
        ("Workflow", "Menus.Workflow", "ApprovalDelegation", "/workflow/approval-delegations", "Menus.Workflow.ApprovalDelegation", 10),
        ("Notifications", "Menus.Notifications", "Notification", "/notifications/notifications", "Menus.Notifications.Notification", 1),
        ("Notifications", "Menus.Notifications", "NotificationRecipient", "/notifications/notification-recipients", "Menus.Notifications.NotificationRecipient", 2),
        ("Notifications", "Menus.Notifications", "NotificationPreference", "/notifications/notification-preferences", "Menus.Notifications.NotificationPreference", 3),
        ("Reporting", "Menus.Reporting", "ReportDefinition", "/reporting/report-definitions", "Menus.Reporting.ReportDefinition", 1),
        ("Reporting", "Menus.Reporting", "DashboardWidget", "/reporting/dashboard-widgets", "Menus.Reporting.DashboardWidget", 2),
    ];

    /// <summary>Modül menüsünün altına per-entity menü girdilerini idempotent ekler.</summary>
    private async Task EnsureEntityMenusAsync(CancellationToken ct)
    {
        foreach (var (module, parentKey, _, route, nameKey, order) in ModuleEntityMenus)
        {
            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);
            if (parent is null)
            {
                continue;
            }
            await EnsureMenuAsync(nameKey, parent.Id, route, "doc", 100 + order, $"{module}.ReadAll", ct);
        }
        _logger.LogInformation("Seeding: {Count} per-entity module menu(s) ensured.", ModuleEntityMenus.Length);
    }

    #endregion

    #region 04 | MENULER | Surec (Process)

    /// <summary>(Module, ParentMenuNameKey, Process, Route, NameKey, Permission, Order)</summary>
    private static readonly (string Module, string ParentKey, string Process, string Route, string NameKey, string Permission, int Order)[] ModuleProcessMenus =
    [
        ("Workflow", "Menus.Workflow", "Approval", "/workflow/processes/approval", "Menus.Workflow.Processes.Approval", "Workflow.Read", 1),
        ("Inventory", "Menus.Inventory", "StockIssue", "/inventory/processes/stock-issue", "Menus.Inventory.Processes.StockIssue", "Inventory.Approve", 2),
        ("Inventory", "Menus.Inventory", "StockTransfer", "/inventory/processes/stock-transfer", "Menus.Inventory.Processes.StockTransfer", "Inventory.Transfer", 3),
        ("Procurement", "Menus.Procurement", "GoodsReceipt", "/procurement/processes/goods-receipt", "Menus.Procurement.Processes.GoodsReceipt", "Procurement.Approve", 4),
        ("Finance", "Menus.Finance", "TimesheetCost", "/finance/processes/timesheet-cost", "Menus.Finance.Processes.TimesheetCost", "Finance.Create", 5),
        ("Finance", "Menus.Finance", "ProgressPaymentPosting", "/finance/processes/progress-payment-posting", "Menus.Finance.Processes.ProgressPaymentPosting", "Finance.Create", 6),
        ("Finance", "Menus.Finance", "PaymentAllocation", "/finance/processes/payment-allocation", "Menus.Finance.Processes.PaymentAllocation", "Finance.Update", 7),
        ("Documents", "Menus.Documents", "Files", "/documents/files", "Menus.Documents.Files", "Documents.Read", 7),
    ];

    /// <summary>Modül menüsünün altına per-process menü girdilerini idempotent ekler.</summary>
    private async Task EnsureProcessMenusAsync(CancellationToken ct)
    {
        foreach (var (_, parentKey, _, route, nameKey, permission, order) in ModuleProcessMenus)
        {
            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);
            if (parent is null)
            {
                continue;
            }
            await EnsureMenuAsync(nameKey, parent.Id, route, "todo", 200 + order, permission, ct);
        }
        _logger.LogInformation("Seeding: {Count} per-process menu(s) ensured.", ModuleProcessMenus.Length);
    }

    #endregion

    #region 05 | MENULER | Rapor (Report)

    /// <summary>(Module, ParentMenuNameKey, Report, Route, NameKey, Order)</summary>
    private static readonly (string Module, string ParentKey, string Report, string Route, string NameKey, int Order)[] ModuleReportMenus =
    [
        ("Procurement", "Menus.Procurement", "PurchaseOrderSummary", "/procurement/reports/purchase-order-summary", "Menus.Procurement.Reports.PurchaseOrderSummary", 1),
        ("Inventory", "Menus.Inventory", "StockBalanceReport", "/inventory/reports/stock-balance-report", "Menus.Inventory.Reports.StockBalanceReport", 2),
        ("Projects", "Menus.Projects", "ProjectStatusReport", "/projects/reports/project-status-report", "Menus.Projects.Reports.ProjectStatusReport", 3),
        ("HR", "Menus.HR", "TimesheetSummary", "/h-r/reports/timesheet-summary", "Menus.HR.Reports.TimesheetSummary", 4),
        ("Finance", "Menus.Finance", "PayableAging", "/finance/reports/payable-aging", "Menus.Finance.Reports.PayableAging", 5),
        ("Finance", "Menus.Finance", "ReceivableAging", "/finance/reports/receivable-aging", "Menus.Finance.Reports.ReceivableAging", 6),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPaymentSummary", "/progress-payments/reports/progress-payment-summary", "Menus.ProgressPayments.Reports.ProgressPaymentSummary", 7),
    ];

    /// <summary>Modül menüsünün altına per-report menü girdilerini idempotent ekler.</summary>
    private async Task EnsureReportMenusAsync(CancellationToken ct)
    {
        foreach (var (module, parentKey, report, route, nameKey, order) in ModuleReportMenus)
        {
            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);
            if (parent is null)
            {
                continue;
            }
            await EnsureMenuAsync(nameKey, parent.Id, route, "chart", 300 + order, $"{module}.{report}.Read", ct);
        }
        _logger.LogInformation("Seeding: {Count} per-report menu(s) ensured.", ModuleReportMenus.Length);
    }

    #endregion

    #region 06 | ORNEK IS VERISI (cekirdek anchor graf)

    private const string DemoMarker = "SEED-DEMO";

    private async Task EnsureSampleBusinessDataAsync(CancellationToken ct)
    {
        // Referans veriler (para birimi, ölçü birimi) önce tohumlanmış olmalı.
        var currency = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "TRY", ct);
        var unit = await _db.UnitsOfMeasure.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == "Piece", ct);
        if (currency is null || unit is null)
        {
            _logger.LogWarning("Sample business data skipped: reference currency/unit not found.");
            return;
        }

        // 1) Şirket
        var company = await GetOrAddAsync(_db.Companies, c => c.Code == "DEMO-CO", () => new Company
        {
            Id = Guid.NewGuid(), Code = "DEMO-CO", Name = "Demo İnşaat A.Ş.",
            BaseCurrencyId = currency.Id, IsActive = true,
        }, ct);

        // 2) Proje türü + durumu + proje
        var projectType = await GetOrAddAsync(_db.ProjectTypes, t => t.Code == "CONSTR", () => new ProjectType
        {
            Id = Guid.NewGuid(), Code = "CONSTR", Name = "İnşaat", IsActive = true,
        }, ct);
        var projectStatus = await GetOrAddAsync(_db.ProjectStatuses, s => s.Code == "ACTIVE", () => new ProjectStatus
        {
            Id = Guid.NewGuid(), Code = "ACTIVE", Name = "Aktif", DisplayOrder = 1, IsClosedState = false, IsActive = true,
        }, ct);
        var project = await GetOrAddAsync(_db.Projects, p => p.Code == "PRJ-001", () => new Project
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, ProjectTypeId = projectType.Id, StatusId = projectStatus.Id,
            Code = "PRJ-001", Name = "Merkez Saha Projesi", StartDate = DateTime.UtcNow.AddMonths(-2),
        }, ct);

        // 3) Tedarikçi (cari)
        var supplier = await GetOrAddAsync(_db.BusinessPartners, b => b.Code == "SUP-001", () => new BusinessPartner
        {
            Id = Guid.NewGuid(), PartnerType = PartnerType.Supplier, Code = "SUP-001",
            Name = "Anadolu Malzeme Ltd.", IsActive = true,
        }, ct);

        // 4) Malzeme kategorisi + iki malzeme
        var category = await GetOrAddAsync(_db.MaterialCategories, c => c.Code == "CAT-001", () => new MaterialCategory
        {
            Id = Guid.NewGuid(), Code = "CAT-001", Name = "Genel Malzeme", IsActive = true,
        }, ct);
        var material1 = await GetOrAddAsync(_db.Materials, m => m.Code == "MAT-001", () => new Material
        {
            Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = unit.Id,
            Code = "MAT-001", Name = "Çimento 50kg", IsActive = true,
        }, ct);
        var material2 = await GetOrAddAsync(_db.Materials, m => m.Code == "MAT-002", () => new Material
        {
            Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = unit.Id,
            Code = "MAT-002", Name = "İnşaat Demiri 12mm", IsActive = true,
        }, ct);

        // 5) Depo
        var warehouse = await GetOrAddAsync(_db.Warehouses, w => w.Code == "WH-001", () => new Warehouse
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, ProjectId = project.Id,
            WarehouseType = WarehouseType.Central, Code = "WH-001", Name = "Merkez Depo", IsActive = true,
        }, ct);

        // 6) Stok bakiyeleri: biri tükenmiş (LowStock = kullanılabilir ≤ 0), biri sağlıklı.
        await EnsureAsync(_db.StockBalances,
            b => b.WarehouseId == warehouse.Id && b.MaterialId == material1.Id,
            () => new StockBalance
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material1.Id,
                Quantity = 0m, ReservedQuantity = 0m, TotalCost = 0m, LastRecalculatedAt = DateTime.UtcNow,
            }, ct);
        await EnsureAsync(_db.StockBalances,
            b => b.WarehouseId == warehouse.Id && b.MaterialId == material2.Id,
            () => new StockBalance
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material2.Id,
                Quantity = 100m, ReservedQuantity = 0m, TotalCost = 145000m, LastRecalculatedAt = DateTime.UtcNow,
            }, ct);

        // 7) İş emri türü + bir açık + bir kapalı iş emri (WorkOrderProgress = açık sayısı).
        var workOrderType = await GetOrAddAsync(_db.WorkOrderTypes, t => t.Code == "WOT-001", () => new WorkOrderType
        {
            Id = Guid.NewGuid(), Code = "WOT-001", Name = "Saha İşi", IsActive = true,
        }, ct);
        await GetOrAddAsync(_db.WorkOrders, w => w.WorkOrderNo == "WO-001", () => new WorkOrder
        {
            Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id,
            Status = WorkOrderStatus.InProgress, WorkOrderNo = "WO-001", Title = "Temel kazısı",
            PlannedStart = DateTime.UtcNow.AddDays(-5), PlannedEnd = DateTime.UtcNow.AddDays(5),
        }, ct);
        await GetOrAddAsync(_db.WorkOrders, w => w.WorkOrderNo == "WO-002", () => new WorkOrder
        {
            Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id,
            Status = WorkOrderStatus.Completed, WorkOrderNo = "WO-002", Title = "Saha temizliği",
            PlannedStart = DateTime.UtcNow.AddDays(-20), PlannedEnd = DateTime.UtcNow.AddDays(-10),
        }, ct);

        // 8) Satın alma siparişi + satır (OrderDelivery = onaylı/kısmen teslim).
        var purchaseOrder = await GetOrAddAsync(_db.PurchaseOrders, o => o.OrderNo == "PO-001", () => new PurchaseOrder
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, ProjectId = project.Id, CurrencyId = currency.Id,
            Status = PurchaseOrderStatus.Approved, OrderNo = "PO-001", OrderDate = DateTime.UtcNow.AddDays(-3),
        }, ct);
        await EnsureAsync(_db.PurchaseOrderLines,
            l => l.PurchaseOrderId == purchaseOrder.Id,
            () => new PurchaseOrderLine
            {
                Id = Guid.NewGuid(), PurchaseOrderId = purchaseOrder.Id, MaterialId = material1.Id,
                Quantity = 50m, UnitPrice = 120m, CurrencyId = currency.Id, ReceivedQuantity = 0m,
            }, ct);

        // 9) Bütçe + satır + (planı aşan) finansal hareket (BudgetOverrun = aşan bütçe sayısı).
        var budget = await GetOrAddAsync(_db.Budgets, b => b.Name == "PRJ-001 Bütçesi", () => new BudgetEntity
        {
            Id = Guid.NewGuid(), ProjectId = project.Id, CurrencyId = currency.Id,
            Name = "PRJ-001 Bütçesi", Year = DateTime.UtcNow.Year, IsActive = true,
        }, ct);
        await EnsureAsync(_db.BudgetLines, l => l.BudgetId == budget.Id, () => new BudgetLine
        {
            Id = Guid.NewGuid(), BudgetId = budget.Id, ProjectId = project.Id,
            Description = "Malzeme bütçesi", PlannedAmount = 100000m,
        }, ct);
        var overrunTx = await GetOrAddAsync(_db.FinancialTransactions, t => t.Description == DemoMarker, () => new FinancialTransaction
        {
            Id = Guid.NewGuid(), TransactionType = FinancialTransactionType.Expense, ProjectId = project.Id,
            CurrencyId = currency.Id, Amount = 130000m, TransactionDate = DateTime.UtcNow.AddDays(-1), Description = DemoMarker,
        }, ct);
        await EnsureAsync(_db.FinancialTransactionLines,
            l => l.FinancialTransactionId == overrunTx.Id,
            () => new FinancialTransactionLine
            {
                Id = Guid.NewGuid(), FinancialTransactionId = overrunTx.Id, ProjectId = project.Id,
                Amount = 130000m, Description = "Gerçekleşen malzeme gideri",
            }, ct);

        // 10) Bekleyen onay talebi (PendingApprovals). Satın alma onay akışının yürürlükteki versiyonunu kullan.
        var pendingExists = await _db.ApprovalRequests.IgnoreQueryFilters()
            .AnyAsync(a => a.RelatedModule == "Procurement" && a.RelatedEntityId == purchaseOrder.Id, ct);
        if (!pendingExists)
        {
            var version = await (from v in _db.ApprovalDefinitionVersions.IgnoreQueryFilters()
                                 join d in _db.ApprovalDefinitions.IgnoreQueryFilters() on v.ApprovalDefinitionId equals d.Id
                                 where d.Code == "PurchaseOrderApproval" && v.IsActive
                                 select v).FirstOrDefaultAsync(ct);
            var requester = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "admin", ct);
            if (version is not null && requester is not null)
            {
                _db.ApprovalRequests.Add(new ApprovalRequest
                {
                    Id = Guid.NewGuid(),
                    ApprovalDefinitionVersionId = version.Id,
                    RelatedModule = "Procurement",
                    RelatedEntityType = "PurchaseOrder",
                    RelatedEntityId = purchaseOrder.Id,
                    RequestedByUserId = requester.Id,
                    Status = ApprovalRequestStatus.Pending,
                    CurrentStepNo = 1,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>Predicate ile eşleşen kaydı döndürür; yoksa fabrikadan üretip ekler ve kaydederek döndürür.</summary>
    private async Task<TEntity> GetOrAddAsync<TEntity>(
        DbSet<TEntity> set,
        Expression<Func<TEntity, bool>> predicate,
        Func<TEntity> factory,
        CancellationToken ct)
        where TEntity : class
    {
        var existing = await set.IgnoreQueryFilters().FirstOrDefaultAsync(predicate, ct);
        if (existing is not null)
        {
            return existing;
        }

        var entity = factory();
        set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    /// <summary>Predicate ile eşleşen kayıt yoksa fabrikadan üretip ekler (kaydı en sonda toplu yapılır).</summary>
    private async Task EnsureAsync<TEntity>(
        DbSet<TEntity> set,
        Expression<Func<TEntity, bool>> predicate,
        Func<TEntity> factory,
        CancellationToken ct)
        where TEntity : class
    {
        if (!await set.IgnoreQueryFilters().AnyAsync(predicate, ct))
        {
            set.Add(factory());
        }
    }

    #endregion

    #region 07 | MODUL SEEDS | Tam ornek veri (tum tablolar)

    private async Task EnsureFullSampleDataAsync(CancellationToken ct)
    {
        // ---- Çapa (anchor) kayıtlar — çekirdek demo grafiğinden ----
        var currency = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "TRY", ct);
        var usd = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "USD", ct);
        var unit = await _db.UnitsOfMeasure.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == "Piece", ct);
        var packageUnit = await _db.UnitsOfMeasure.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == "Package", ct);
        var admin = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "admin", ct);
        var company = await _db.Companies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "DEMO-CO", ct);
        var project = await _db.Projects.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Code == "PRJ-001", ct);
        var supplier = await _db.BusinessPartners.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "SUP-001", ct);
        var category = await _db.MaterialCategories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "CAT-001", ct);
        var material1 = await _db.Materials.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Code == "MAT-001", ct);
        var material2 = await _db.Materials.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Code == "MAT-002", ct);
        var warehouse = await _db.Warehouses.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Code == "WH-001", ct);
        var workOrderType = await _db.WorkOrderTypes.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Code == "WOT-001", ct);
        var workOrder = await _db.WorkOrders.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.WorkOrderNo == "WO-001", ct);
        var purchaseOrder = await _db.PurchaseOrders.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.OrderNo == "PO-001", ct);

        if (currency is null || unit is null || admin is null || company is null || project is null ||
            supplier is null || category is null || material1 is null || material2 is null ||
            warehouse is null || workOrderType is null || workOrder is null || purchaseOrder is null)
        {
            _logger.LogWarning("Full sample data skipped: one or more core anchor records are missing.");
            return;
        }

        // İkincil demo kullanıcılar (rol şablonlarından gelir); yoksa admin'e düşülür.
        var secondUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == "ops.manager@energy.local", ct) ?? admin;
        var thirdUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == "basic.user@energy.local", ct) ?? admin;

        await SeedCoreExtrasAsync(company, currency, usd, unit, packageUnit, ct);
        var (department, employee) = await SeedOrganizationAsync(company, project, currency, admin, ct);
        var customer = await SeedBusinessPartnerDetailsAsync(supplier, currency, ct);
        var (projectPhase, _) = await SeedProjectDetailsAsync(project, admin, employee, ct);
        await SeedCatalogDetailsAsync(category, material1, unit, packageUnit, ct);
        var equipment = await SeedAssetsAsync(company, project, employee, warehouse, ct);
        await SeedInventoryFlowAsync(company, project, warehouse, material2, unit, currency, workOrder, ct);
        var requestLine = await SeedRequestsAsync(project, material1, unit, admin, ct);
        await SeedProcurementExtrasAsync(supplier, purchaseOrder, warehouse, material1, currency, requestLine, ct);
        await SeedOperationsDetailsAsync(workOrder, material1, employee, admin, ct);
        await SeedFieldOperationsAsync(project, projectPhase, workOrder, employee, equipment, material1, ct);
        await SeedHrAsync(employee, project, workOrder, ct);
        var (financialAccount, costCenter, payable, receivable) =
            await SeedFinanceAccountsAndOpenItemsAsync(supplier, customer, currency, ct);
        await SeedFinanceSettlementsAsync(supplier, customer, currency, financialAccount, payable, receivable, ct);
        var (contract, contractLine) = await SeedContractsAsync(project, currency, customer, ct);
        await SeedProgressPaymentsAsync(contract, contractLine, customer, ct);
        await SeedDocumentsAsync(project, admin, ct);
        await SeedWorkflowExtrasAsync(purchaseOrder, admin, secondUser, ct);
        await SeedNotificationsAsync(material1, admin, ct);
        await SeedReportingAsync(ct);
        await SeedDirectUserGrantsAndAuditAsync(admin, thirdUser, ct);
        await SeedChatAsync(admin, secondUser, ct);

        _logger.LogInformation("Full sample data: every table populated with at least one demo record.");
    }

    // =====================================================================================
    //  Core — şube, departman, kur, birim dönüşümü, sıra tanımı, sistem ayarı
    // =====================================================================================
    private async Task SeedCoreExtrasAsync(
        Company company, Currency currency, Currency? usd, UnitOfMeasure unit, UnitOfMeasure? packageUnit, CancellationToken ct)
    {
        await GetOrAddAsync(_db.Branches, b => b.Code == "BR-001", () => new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "BR-001", Name = "Merkez Şube",
            Address = "Ankara", IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.Departments, d => d.Code == "DEP-001", () => new Department
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "DEP-001", Name = "Saha Operasyonları", IsActive = true,
        }, ct);

        if (usd is not null)
        {
            await GetOrAddAsync(_db.ExchangeRates, r => r.CurrencyId == usd.Id, () => new ExchangeRate
            {
                Id = Guid.NewGuid(), CurrencyId = usd.Id, RateDate = DateTime.UtcNow.Date, Rate = 32.50m,
            }, ct);
        }

        if (packageUnit is not null)
        {
            await GetOrAddAsync(_db.UnitConversions,
                c => c.FromUnitOfMeasureId == packageUnit.Id && c.ToUnitOfMeasureId == unit.Id,
                () => new UnitConversion
                {
                    Id = Guid.NewGuid(), FromUnitOfMeasureId = packageUnit.Id, ToUnitOfMeasureId = unit.Id, Factor = 12m,
                }, ct);
        }

        await GetOrAddAsync(_db.SequenceDefinitions,
            s => s.Module == "Procurement" && s.EntityType == "PurchaseOrder",
            () => new SequenceDefinition
            {
                Id = Guid.NewGuid(), Module = "Procurement", EntityType = "PurchaseOrder",
                Prefix = "PO-", Padding = 6, NextNumber = 2, Format = "{Prefix}{Number}",
            }, ct);

        await GetOrAddAsync(_db.SystemSettings, s => s.Key == "Demo.DefaultCompany", () => new SystemSetting
        {
            Id = Guid.NewGuid(), Key = "Demo.DefaultCompany", Value = company.Code, Category = "Demo",
            DescriptionKey = "SystemSettings.Demo.DefaultCompany.Description",
        }, ct);
    }

    // =====================================================================================
    //  Organization — pozisyon, yetkinlik, personel, yetkinlik ataması, izin, masraf
    // =====================================================================================
    private async Task<(Department Department, Employee Employee)> SeedOrganizationAsync(
        Company company, Project project, Currency currency, User admin, CancellationToken ct)
    {
        var department = await _db.Departments.IgnoreQueryFilters().FirstAsync(d => d.Code == "DEP-001", ct);
        var branch = await _db.Branches.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "BR-001", ct);

        var position = await GetOrAddAsync(_db.EmployeePositions, p => p.Code == "POS-001", () => new EmployeePosition
        {
            Id = Guid.NewGuid(), Code = "POS-001", Name = "Saha Mühendisi", IsActive = true,
        }, ct);

        var skill = await GetOrAddAsync(_db.EmployeeSkills, s => s.Code == "SKL-001", () => new EmployeeSkill
        {
            Id = Guid.NewGuid(), Code = "SKL-001", Name = "Kaynakçılık", IsActive = true,
        }, ct);

        var employee = await GetOrAddAsync(_db.Employees, e => e.Code == "EMP-001", () => new Employee
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch?.Id, DepartmentId = department.Id,
            EmployeePositionId = position.Id, UserId = admin.Id, Code = "EMP-001",
            FirstName = "Ali", LastName = "Usta", Phone = "5550000001", Email = "ali.usta@energy.local",
            HireDate = DateTime.UtcNow.AddYears(-1), IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.EmployeeSkillAssignments,
            a => a.EmployeeId == employee.Id && a.EmployeeSkillId == skill.Id,
            () => new EmployeeSkillAssignment
            {
                Id = Guid.NewGuid(), EmployeeId = employee.Id, EmployeeSkillId = skill.Id, Level = 4, Note = "Sertifikalı",
            }, ct);

        await GetOrAddAsync(_db.LeaveRequests,
            l => l.EmployeeId == employee.Id && l.LeaveType == "Annual",
            () => new LeaveRequest
            {
                Id = Guid.NewGuid(), EmployeeId = employee.Id, LeaveType = "Annual",
                StartDate = DateTime.UtcNow.AddDays(10), EndDate = DateTime.UtcNow.AddDays(14), Days = 5m,
                Reason = "Yıllık izin", Status = ApprovalRequestStatus.Pending,
            }, ct);

        var expenseClaim = await GetOrAddAsync(_db.ExpenseClaims, c => c.ClaimNo == "EXP-001", () => new ExpenseClaim
        {
            Id = Guid.NewGuid(), EmployeeId = employee.Id, ProjectId = project.Id, CurrencyId = currency.Id,
            ClaimNo = "EXP-001", ClaimDate = DateTime.UtcNow.AddDays(-2), TotalAmount = 750m,
            Status = ApprovalRequestStatus.Pending,
        }, ct);

        await GetOrAddAsync(_db.ExpenseClaimLines,
            l => l.ExpenseClaimId == expenseClaim.Id,
            () => new ExpenseClaimLine
            {
                Id = Guid.NewGuid(), ExpenseClaimId = expenseClaim.Id, Description = "Yakıt gideri",
                ExpenseDate = DateTime.UtcNow.AddDays(-2), Amount = 750m, Category = "Travel",
            }, ct);

        return (department, employee);
    }

    // =====================================================================================
    //  BusinessPartners — müşteri cari + iletişim, adres, banka hesabı
    // =====================================================================================
    private async Task<BusinessPartner> SeedBusinessPartnerDetailsAsync(
        BusinessPartner supplier, Currency currency, CancellationToken ct)
    {
        var customer = await GetOrAddAsync(_db.BusinessPartners, b => b.Code == "CUS-001", () => new BusinessPartner
        {
            Id = Guid.NewGuid(), PartnerType = PartnerType.Customer, Code = "CUS-001",
            Name = "Marmara Enerji A.Ş.", TaxNumber = "1234567890", Phone = "5550000010",
            Email = "info@marmaraenerji.local", IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.BusinessPartnerContacts,
            c => c.BusinessPartnerId == supplier.Id,
            () => new BusinessPartnerContact
            {
                Id = Guid.NewGuid(), BusinessPartnerId = supplier.Id, FullName = "Hasan Tedarik",
                Title = "Satış Müdürü", Phone = "5550000002", Email = "hasan@anadolumalzeme.local", IsPrimary = true,
            }, ct);

        await GetOrAddAsync(_db.BusinessPartnerAddresses,
            a => a.BusinessPartnerId == supplier.Id,
            () => new BusinessPartnerAddress
            {
                Id = Guid.NewGuid(), BusinessPartnerId = supplier.Id, AddressType = "Billing",
                AddressLine = "Organize Sanayi Bölgesi No:12", City = "Kocaeli", Country = "Türkiye",
                PostalCode = "41000", IsPrimary = true,
            }, ct);

        await GetOrAddAsync(_db.BusinessPartnerBankAccounts,
            a => a.BusinessPartnerId == supplier.Id,
            () => new BusinessPartnerBankAccount
            {
                Id = Guid.NewGuid(), BusinessPartnerId = supplier.Id, BankName = "Demo Bank",
                Branch = "Merkez", Iban = "TR000000000000000000000001", CurrencyId = currency.Id, IsPrimary = true,
            }, ct);

        return customer;
    }

    // =====================================================================================
    //  Projects — lokasyon, faz, üye, not
    // =====================================================================================
    private async Task<(ProjectPhase Phase, ProjectLocation Location)> SeedProjectDetailsAsync(
        Project project, User admin, Employee employee, CancellationToken ct)
    {
        var location = await GetOrAddAsync(_db.ProjectLocations,
            l => l.ProjectId == project.Id && l.Code == "LOC-001",
            () => new ProjectLocation
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, Code = "LOC-001", Name = "A Blok",
            }, ct);

        var phase = await GetOrAddAsync(_db.ProjectPhases,
            p => p.ProjectId == project.Id && p.Code == "PH-001",
            () => new ProjectPhase
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, Code = "PH-001", Name = "Kaba İnşaat",
                ProgressPercentage = 35m, PlannedStart = DateTime.UtcNow.AddMonths(-1), PlannedEnd = DateTime.UtcNow.AddMonths(2),
            }, ct);

        await GetOrAddAsync(_db.ProjectMembers,
            m => m.ProjectId == project.Id && m.UserId == admin.Id,
            () => new ProjectMember
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, UserId = admin.Id, EmployeeId = employee.Id, ProjectRole = "Manager",
            }, ct);

        await GetOrAddAsync(_db.ProjectNotes,
            n => n.ProjectId == project.Id && n.Title == "Saha başlangıcı",
            () => new ProjectNote
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, Title = "Saha başlangıcı", Body = "Mobilizasyon tamamlandı.",
            }, ct);

        return (phase, location);
    }

    // =====================================================================================
    //  Catalog — marka, dinamik öznitelik tanımı/seçeneği/kategori bağı/değeri, birim dönüşümü
    // =====================================================================================
    private async Task SeedCatalogDetailsAsync(
        MaterialCategory category, Material material, UnitOfMeasure unit, UnitOfMeasure? packageUnit, CancellationToken ct)
    {
        var brand = await GetOrAddAsync(_db.Brands, b => b.Code == "BRD-001", () => new Brand
        {
            Id = Guid.NewGuid(), Code = "BRD-001", Name = "Demo Marka", IsActive = true,
        }, ct);

        // Malzemeye marka bağla (yoksa).
        if (material.BrandId is null)
        {
            material.BrandId = brand.Id;
            await _db.SaveChangesAsync(ct);
        }

        var attribute = await GetOrAddAsync(_db.MaterialAttributeDefinitions, a => a.Code == "ATT-001", () => new MaterialAttributeDefinition
        {
            Id = Guid.NewGuid(), Code = "ATT-001", Name = "Renk", DataType = "Option", IsActive = true,
        }, ct);

        var option = await GetOrAddAsync(_db.MaterialAttributeOptions,
            o => o.MaterialAttributeDefinitionId == attribute.Id && o.Value == "Gri",
            () => new MaterialAttributeOption
            {
                Id = Guid.NewGuid(), MaterialAttributeDefinitionId = attribute.Id, Value = "Gri", DisplayOrder = 1,
            }, ct);

        await GetOrAddAsync(_db.MaterialCategoryAttributes,
            c => c.MaterialCategoryId == category.Id && c.MaterialAttributeDefinitionId == attribute.Id,
            () => new MaterialCategoryAttribute
            {
                Id = Guid.NewGuid(), MaterialCategoryId = category.Id, MaterialAttributeDefinitionId = attribute.Id,
                IsRequired = false, DisplayOrder = 1,
            }, ct);

        await GetOrAddAsync(_db.MaterialAttributeValues,
            v => v.MaterialId == material.Id && v.MaterialAttributeDefinitionId == attribute.Id,
            () => new MaterialAttributeValue
            {
                Id = Guid.NewGuid(), MaterialId = material.Id, MaterialAttributeDefinitionId = attribute.Id, OptionId = option.Id,
            }, ct);

        if (packageUnit is not null)
        {
            await GetOrAddAsync(_db.MaterialUnitConversions,
                c => c.MaterialId == material.Id && c.FromUnitOfMeasureId == packageUnit.Id && c.ToUnitOfMeasureId == unit.Id,
                () => new MaterialUnitConversion
                {
                    Id = Guid.NewGuid(), MaterialId = material.Id, FromUnitOfMeasureId = packageUnit.Id,
                    ToUnitOfMeasureId = unit.Id, Factor = 25m,
                }, ct);
        }
    }

    // =====================================================================================
    //  Assets — ekipman kartı, atama, bakım
    // =====================================================================================
    private async Task<EquipmentAsset> SeedAssetsAsync(
        Company company, Project project, Employee employee, Warehouse warehouse, CancellationToken ct)
    {
        var equipment = await GetOrAddAsync(_db.EquipmentAssets, e => e.Code == "EQ-001", () => new EquipmentAsset
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "EQ-001", Name = "Ekskavatör",
            AssetType = "Machine", SerialNo = "SN-EQ-001", PurchaseDate = DateTime.UtcNow.AddYears(-2), IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.EquipmentAssignments,
            a => a.EquipmentAssetId == equipment.Id && a.ProjectId == project.Id,
            () => new EquipmentAssignment
            {
                Id = Guid.NewGuid(), EquipmentAssetId = equipment.Id, ProjectId = project.Id, EmployeeId = employee.Id,
                WarehouseId = warehouse.Id, StartDate = DateTime.UtcNow.AddMonths(-1), IsActive = true,
            }, ct);

        await GetOrAddAsync(_db.EquipmentMaintenances,
            m => m.EquipmentAssetId == equipment.Id,
            () => new EquipmentMaintenance
            {
                Id = Guid.NewGuid(), EquipmentAssetId = equipment.Id, MaintenanceType = "Planned",
                ScheduledDate = DateTime.UtcNow.AddDays(30), Cost = 1500m, Note = "Periyodik bakım",
            }, ct);

        return equipment;
    }

    // =====================================================================================
    //  Inventory — lokasyon, belge türleri, giriş+çıkış belgesi/satırı, lot, hareket,
    //  FIFO dağılımı, rezervasyon, sayım, depolar arası transfer
    // =====================================================================================
    private async Task SeedInventoryFlowAsync(
        Company company, Project project, Warehouse warehouse, Material material, UnitOfMeasure unit,
        Currency currency, WorkOrder workOrder, CancellationToken ct)
    {
        await GetOrAddAsync(_db.WarehouseLocations,
            l => l.WarehouseId == warehouse.Id && l.Code == "WL-001",
            () => new WarehouseLocation
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, Code = "WL-001", Name = "Raf A1",
            }, ct);

        var inType = await GetOrAddAsync(_db.StockDocumentTypes, t => t.Code == "SDT-IN", () => new StockDocumentType
        {
            Id = Guid.NewGuid(), Code = "SDT-IN", Name = "Mal Girişi", Direction = "In", IsActive = true,
        }, ct);
        var outType = await GetOrAddAsync(_db.StockDocumentTypes, t => t.Code == "SDT-OUT", () => new StockDocumentType
        {
            Id = Guid.NewGuid(), Code = "SDT-OUT", Name = "Sarf Çıkışı", Direction = "Out", IsActive = true,
        }, ct);

        // Giriş belgesi + satırı + lot + (+) hareket.
        var inDoc = await GetOrAddAsync(_db.StockDocuments, d => d.DocumentNo == "SD-001", () => new StockDocument
        {
            Id = Guid.NewGuid(), DocumentTypeId = inType.Id, TargetWarehouseId = warehouse.Id, ProjectId = project.Id,
            Status = DocumentStatus.Approved, DocumentNo = "SD-001", DocumentDate = DateTime.UtcNow.AddDays(-7), Note = "İlk giriş",
        }, ct);
        var inLine = await GetOrAddAsync(_db.StockDocumentLines,
            l => l.StockDocumentId == inDoc.Id,
            () => new StockDocumentLine
            {
                Id = Guid.NewGuid(), StockDocumentId = inDoc.Id, MaterialId = material.Id, UnitOfMeasureId = unit.Id,
                Quantity = 100m, UnitPrice = 1450m, CurrencyId = currency.Id, Note = "Açılış stoğu",
            }, ct);
        var lot = await GetOrAddAsync(_db.StockLots, l => l.LotNo == "LOT-001", () => new StockLot
        {
            Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material.Id, SourceStockDocumentLineId = inLine.Id,
            LotNo = "LOT-001", InitialQuantity = 100m, RemainingQuantity = 90m, UnitCost = 1450m, ReceivedAt = DateTime.UtcNow.AddDays(-7),
        }, ct);
        await GetOrAddAsync(_db.StockTransactions,
            t => t.StockDocumentLineId == inLine.Id,
            () => new StockTransaction
            {
                Id = Guid.NewGuid(), StockDocumentId = inDoc.Id, StockDocumentLineId = inLine.Id, StockLotId = lot.Id,
                WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = 100m, UnitCost = 1450m, TransactionDate = DateTime.UtcNow.AddDays(-7),
            }, ct);

        // Çıkış belgesi + satırı + FIFO dağılımı + (-) hareket.
        var outDoc = await GetOrAddAsync(_db.StockDocuments, d => d.DocumentNo == "SD-002", () => new StockDocument
        {
            Id = Guid.NewGuid(), DocumentTypeId = outType.Id, SourceWarehouseId = warehouse.Id, ProjectId = project.Id,
            Status = DocumentStatus.Approved, DocumentNo = "SD-002", DocumentDate = DateTime.UtcNow.AddDays(-3), Note = "Sahaya sarf",
        }, ct);
        var outLine = await GetOrAddAsync(_db.StockDocumentLines,
            l => l.StockDocumentId == outDoc.Id,
            () => new StockDocumentLine
            {
                Id = Guid.NewGuid(), StockDocumentId = outDoc.Id, MaterialId = material.Id, UnitOfMeasureId = unit.Id,
                Quantity = 10m, UnitPrice = 1450m, CurrencyId = currency.Id, Note = "Sarf",
            }, ct);
        await GetOrAddAsync(_db.StockIssueAllocations,
            a => a.StockDocumentLineId == outLine.Id,
            () => new StockIssueAllocation
            {
                Id = Guid.NewGuid(), StockDocumentLineId = outLine.Id, StockLotId = lot.Id, Quantity = 10m, UnitCost = 1450m,
            }, ct);
        await GetOrAddAsync(_db.StockTransactions,
            t => t.StockDocumentLineId == outLine.Id,
            () => new StockTransaction
            {
                Id = Guid.NewGuid(), StockDocumentId = outDoc.Id, StockDocumentLineId = outLine.Id, StockLotId = lot.Id,
                WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = -10m, UnitCost = 1450m, TransactionDate = DateTime.UtcNow.AddDays(-3),
            }, ct);

        // Rezervasyon (iş emrine).
        await GetOrAddAsync(_db.StockReservations,
            r => r.MaterialId == material.Id && r.RelatedEntityId == workOrder.Id,
            () => new StockReservation
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = 5m,
                RelatedModule = "Operations", RelatedEntityType = "WorkOrder", RelatedEntityId = workOrder.Id, IsReleased = false,
            }, ct);

        // Sayım başlığı + satırı.
        var count = await GetOrAddAsync(_db.StockCounts, c => c.CountNo == "SC-001", () => new StockCount
        {
            Id = Guid.NewGuid(), WarehouseId = warehouse.Id, CountNo = "SC-001", CountDate = DateTime.UtcNow.AddDays(-1),
            Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.StockCountLines,
            l => l.StockCountId == count.Id,
            () => new StockCountLine
            {
                Id = Guid.NewGuid(), StockCountId = count.Id, MaterialId = material.Id, SystemQuantity = 90m, CountedQuantity = 89m,
            }, ct);

        // Depolar arası transfer (ikinci depo gerekli).
        var warehouse2 = await GetOrAddAsync(_db.Warehouses, w => w.Code == "WH-002", () => new Warehouse
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, ProjectId = project.Id, WarehouseType = WarehouseType.ProjectSite,
            Code = "WH-002", Name = "Saha Deposu", IsActive = true,
        }, ct);
        var transfer = await GetOrAddAsync(_db.WarehouseTransfers, t => t.TransferNo == "WT-001", () => new WarehouseTransfer
        {
            Id = Guid.NewGuid(), SourceWarehouseId = warehouse.Id, TargetWarehouseId = warehouse2.Id,
            TransferNo = "WT-001", TransferDate = DateTime.UtcNow.AddDays(-2), Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.WarehouseTransferLines,
            l => l.WarehouseTransferId == transfer.Id,
            () => new WarehouseTransferLine
            {
                Id = Guid.NewGuid(), WarehouseTransferId = transfer.Id, MaterialId = material.Id, Quantity = 20m,
            }, ct);
    }

    // =====================================================================================
    //  Requests — talep türü, talep başlığı, talep satırı
    // =====================================================================================
    private async Task<RequestLine> SeedRequestsAsync(
        Project project, Material material, UnitOfMeasure unit, User admin, CancellationToken ct)
    {
        var requestType = await GetOrAddAsync(_db.RequestTypes, t => t.Code == "RQT-001", () => new RequestType
        {
            Id = Guid.NewGuid(), Code = "RQT-001", Name = "Malzeme Talebi", Category = "Material", IsActive = true,
        }, ct);

        var request = await GetOrAddAsync(_db.Requests, r => r.RequestNo == "REQ-001", () => new Request
        {
            Id = Guid.NewGuid(), RequestTypeId = requestType.Id, ProjectId = project.Id, RequestedByUserId = admin.Id,
            Status = RequestStatus.Approved, RequestNo = "REQ-001", RequestDate = DateTime.UtcNow.AddDays(-6),
            Description = "Saha malzeme ihtiyacı",
        }, ct);

        return await GetOrAddAsync(_db.RequestLines,
            l => l.RequestId == request.Id,
            () => new RequestLine
            {
                Id = Guid.NewGuid(), RequestId = request.Id, MaterialId = material.Id, Quantity = 50m,
                UnitOfMeasureId = unit.Id, Note = "Acil",
            }, ct);
    }

    // =====================================================================================
    //  Procurement — teklif+satır, mal kabul+satır, tedarikçi faturası+satır
    // =====================================================================================
    private async Task SeedProcurementExtrasAsync(
        BusinessPartner supplier, PurchaseOrder purchaseOrder, Warehouse warehouse, Material material,
        Currency currency, RequestLine requestLine, CancellationToken ct)
    {
        var quote = await GetOrAddAsync(_db.SupplierQuotes, q => q.QuoteNo == "SQ-001", () => new SupplierQuote
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, CurrencyId = currency.Id, QuoteNo = "SQ-001",
            QuoteDate = DateTime.UtcNow.AddDays(-5), PaymentTerm = "30 gün", Status = DocumentStatus.Approved, IsSelected = true,
        }, ct);
        await GetOrAddAsync(_db.SupplierQuoteLines,
            l => l.SupplierQuoteId == quote.Id,
            () => new SupplierQuoteLine
            {
                Id = Guid.NewGuid(), SupplierQuoteId = quote.Id, RequestLineId = requestLine.Id, MaterialId = material.Id,
                Description = "Teklif kalemi", Quantity = 50m, UnitPrice = 118m, TaxRate = 20m, DiscountRate = 5m, DeliveryDays = 7,
            }, ct);

        var receipt = await GetOrAddAsync(_db.PurchaseReceipts, r => r.ReceiptNo == "PR-001", () => new PurchaseReceipt
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, PurchaseOrderId = purchaseOrder.Id, WarehouseId = warehouse.Id,
            ReceiptNo = "PR-001", ReceiptDate = DateTime.UtcNow.AddDays(-1), Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.PurchaseReceiptLines,
            l => l.PurchaseReceiptId == receipt.Id,
            () => new PurchaseReceiptLine
            {
                Id = Guid.NewGuid(), PurchaseReceiptId = receipt.Id, MaterialId = material.Id, Quantity = 30m, UnitPrice = 120m,
            }, ct);

        var invoice = await GetOrAddAsync(_db.SupplierInvoices, i => i.InvoiceNo == "SI-001", () => new SupplierInvoice
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, PurchaseOrderId = purchaseOrder.Id, PurchaseReceiptId = receipt.Id,
            CurrencyId = currency.Id, InvoiceNo = "SI-001", InvoiceDate = DateTime.UtcNow, TotalAmount = 4320m, Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.SupplierInvoiceLines,
            l => l.SupplierInvoiceId == invoice.Id,
            () => new SupplierInvoiceLine
            {
                Id = Guid.NewGuid(), SupplierInvoiceId = invoice.Id, MaterialId = material.Id,
                Description = "Fatura kalemi", Quantity = 30m, UnitPrice = 120m, TaxRate = 20m,
            }, ct);
    }

    // =====================================================================================
    //  Operations — atama, malzeme planı/kullanımı, kontrol listesi+satırı, durum geçmişi
    // =====================================================================================
    private async Task SeedOperationsDetailsAsync(
        WorkOrder workOrder, Material material, Employee employee, User admin, CancellationToken ct)
    {
        await GetOrAddAsync(_db.WorkOrderAssignments,
            a => a.WorkOrderId == workOrder.Id && a.EmployeeId == employee.Id,
            () => new WorkOrderAssignment
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, EmployeeId = employee.Id, UserId = admin.Id, AssignmentRole = "Lead",
            }, ct);

        await GetOrAddAsync(_db.WorkOrderMaterialPlans,
            p => p.WorkOrderId == workOrder.Id && p.MaterialId == material.Id,
            () => new WorkOrderMaterialPlan
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, MaterialId = material.Id, PlannedQuantity = 40m,
            }, ct);

        await GetOrAddAsync(_db.WorkOrderMaterialUsages,
            u => u.WorkOrderId == workOrder.Id && u.MaterialId == material.Id,
            () => new WorkOrderMaterialUsage
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, MaterialId = material.Id, UsedQuantity = 10m,
            }, ct);

        var checklist = await GetOrAddAsync(_db.WorkOrderChecklists,
            c => c.WorkOrderId == workOrder.Id,
            () => new WorkOrderChecklist
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, Name = "İSG Kontrolleri", IsRequired = true,
            }, ct);
        await GetOrAddAsync(_db.WorkOrderChecklistItems,
            i => i.WorkOrderChecklistId == checklist.Id,
            () => new WorkOrderChecklistItem
            {
                Id = Guid.NewGuid(), WorkOrderChecklistId = checklist.Id, Description = "Baret takıldı mı?",
                IsRequired = true, IsCompleted = true,
            }, ct);

        await GetOrAddAsync(_db.WorkOrderStatusHistories,
            h => h.WorkOrderId == workOrder.Id,
            () => new WorkOrderStatusHistory
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, FromStatus = WorkOrderStatus.Draft,
                ToStatus = WorkOrderStatus.InProgress, ChangedAt = DateTime.UtcNow.AddDays(-5), Note = "Çalışma başladı",
            }, ct);
    }

    // =====================================================================================
    //  FieldOperations — günlük saha raporu (+işçi/ekipman/malzeme), ilerleme, metraj
    // =====================================================================================
    private async Task SeedFieldOperationsAsync(
        Project project, ProjectPhase phase, WorkOrder workOrder, Employee employee, EquipmentAsset equipment,
        Material material, CancellationToken ct)
    {
        var report = await GetOrAddAsync(_db.DailySiteReports, r => r.ReportNo == "DSR-001", () => new DailySiteReport
        {
            Id = Guid.NewGuid(), ProjectId = project.Id, WorkOrderId = workOrder.Id, ReportNo = "DSR-001",
            ReportDate = DateTime.UtcNow.AddDays(-1), Weather = "Açık", Notes = "Çalışma sorunsuz", Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.DailySiteReportWorkers,
            w => w.DailySiteReportId == report.Id,
            () => new DailySiteReportWorker
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, EmployeeId = employee.Id, HoursWorked = 8m, Note = "Tam gün",
            }, ct);
        await GetOrAddAsync(_db.DailySiteReportEquipments,
            e => e.DailySiteReportId == report.Id,
            () => new DailySiteReportEquipment
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, EquipmentAssetId = equipment.Id, Hours = 6m,
            }, ct);
        await GetOrAddAsync(_db.DailySiteReportMaterials,
            m => m.DailySiteReportId == report.Id,
            () => new DailySiteReportMaterial
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, MaterialId = material.Id, Quantity = 10m,
            }, ct);

        await GetOrAddAsync(_db.ProgressEntries,
            p => p.ProjectId == project.Id && p.ProjectPhaseId == phase.Id,
            () => new ProgressEntry
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, ProjectPhaseId = phase.Id, EntryDate = DateTime.UtcNow.AddDays(-1),
                Quantity = 120m, Percentage = 35m, Note = "Kaba inşaat ilerlemesi",
            }, ct);

        var sheet = await GetOrAddAsync(_db.MeasurementSheets, s => s.SheetNo == "MS-001", () => new MeasurementSheet
        {
            Id = Guid.NewGuid(), ProjectId = project.Id, SheetNo = "MS-001", SheetDate = DateTime.UtcNow.AddDays(-1), Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.MeasurementSheetLines,
            l => l.MeasurementSheetId == sheet.Id,
            () => new MeasurementSheetLine
            {
                Id = Guid.NewGuid(), MeasurementSheetId = sheet.Id, Description = "Beton dökümü", Quantity = 120m, UnitPrice = 950m,
            }, ct);
    }

    // =====================================================================================
    //  HR — puantaj başlığı + satırı
    // =====================================================================================
    private async Task SeedHrAsync(Employee employee, Project project, WorkOrder workOrder, CancellationToken ct)
    {
        var timesheet = await GetOrAddAsync(_db.Timesheets, t => t.TimesheetNo == "TS-001", () => new Timesheet
        {
            Id = Guid.NewGuid(), TimesheetNo = "TS-001", PeriodStart = DateTime.UtcNow.AddDays(-7), PeriodEnd = DateTime.UtcNow,
            Status = ApprovalRequestStatus.Pending,
        }, ct);
        await GetOrAddAsync(_db.TimesheetLines,
            l => l.TimesheetId == timesheet.Id,
            () => new TimesheetLine
            {
                Id = Guid.NewGuid(), TimesheetId = timesheet.Id, EmployeeId = employee.Id, ProjectId = project.Id,
                WorkOrderId = workOrder.Id, WorkDate = DateTime.UtcNow.AddDays(-1), NormalHours = 8m, OvertimeHours = 2m, HourlyCost = 150m,
            }, ct);
    }

    // =====================================================================================
    //  Finance — hesap, maliyet merkezi, borç/alacak açık kalemleri
    // =====================================================================================
    private async Task<(FinancialAccount Account, CostCenter CostCenter, Payable Payable, Receivable Receivable)>
        SeedFinanceAccountsAndOpenItemsAsync(BusinessPartner supplier, BusinessPartner customer, Currency currency, CancellationToken ct)
    {
        var account = await GetOrAddAsync(_db.FinancialAccounts, a => a.Code == "FA-001", () => new FinancialAccount
        {
            Id = Guid.NewGuid(), Code = "FA-001", Name = "Merkez Banka Hesabı", AccountType = "Bank", CurrencyId = currency.Id, IsActive = true,
        }, ct);

        var costCenter = await GetOrAddAsync(_db.CostCenters, c => c.Code == "CC-001", () => new CostCenter
        {
            Id = Guid.NewGuid(), Code = "CC-001", Name = "Saha Maliyet Merkezi", IsActive = true,
        }, ct);

        var payable = await GetOrAddAsync(_db.Payables,
            p => p.PartnerId == supplier.Id && !p.IsClosed,
            () => new Payable
            {
                Id = Guid.NewGuid(), PartnerId = supplier.Id, CurrencyId = currency.Id, Amount = 4320m, RemainingAmount = 4320m,
                DueDate = DateTime.UtcNow.AddDays(20), RelatedModule = "Procurement", RelatedEntityType = "SupplierInvoice", IsClosed = false,
            }, ct);

        var receivable = await GetOrAddAsync(_db.Receivables,
            r => r.PartnerId == customer.Id && !r.IsClosed,
            () => new Receivable
            {
                Id = Guid.NewGuid(), PartnerId = customer.Id, CurrencyId = currency.Id, Amount = 50000m, RemainingAmount = 50000m,
                DueDate = DateTime.UtcNow.AddDays(15), RelatedModule = "ProgressPayments", RelatedEntityType = "ProgressPayment", IsClosed = false,
            }, ct);

        return (account, costCenter, payable, receivable);
    }

    // =====================================================================================
    //  Finance — ödeme + borç dağılımı, tahsilat + alacak dağılımı
    // =====================================================================================
    private async Task SeedFinanceSettlementsAsync(
        BusinessPartner supplier, BusinessPartner customer, Currency currency, FinancialAccount account,
        Payable payable, Receivable receivable, CancellationToken ct)
    {
        var payment = await GetOrAddAsync(_db.Payments, p => p.PaymentNo == "PAY-001", () => new Payment
        {
            Id = Guid.NewGuid(), PartnerId = supplier.Id, CurrencyId = currency.Id, FinancialAccountId = account.Id,
            Amount = 2000m, PaymentDate = DateTime.UtcNow.AddDays(-1), PaymentNo = "PAY-001", Status = ApprovalRequestStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.PaymentAllocations,
            a => a.PaymentId == payment.Id,
            () => new PaymentAllocation { Id = Guid.NewGuid(), PaymentId = payment.Id, PayableId = payable.Id, Amount = 2000m }, ct);

        var collection = await GetOrAddAsync(_db.Collections, c => c.CollectionNo == "COL-001", () => new Collection
        {
            Id = Guid.NewGuid(), PartnerId = customer.Id, CurrencyId = currency.Id, FinancialAccountId = account.Id,
            Amount = 15000m, CollectionDate = DateTime.UtcNow.AddDays(-1), CollectionNo = "COL-001", Status = ApprovalRequestStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.CollectionAllocations,
            a => a.CollectionId == collection.Id,
            () => new CollectionAllocation { Id = Guid.NewGuid(), CollectionId = collection.Id, ReceivableId = receivable.Id, Amount = 15000m }, ct);
    }

    // =====================================================================================
    //  Contracts — sözleşme, taraf, kalem, ek protokol
    // =====================================================================================
    private async Task<(Contract Contract, ContractLine Line)> SeedContractsAsync(
        Project project, Currency currency, BusinessPartner customer, CancellationToken ct)
    {
        var contract = await GetOrAddAsync(_db.Contracts, c => c.ContractNo == "CON-001", () => new Contract
        {
            Id = Guid.NewGuid(), ContractType = ContractType.Customer, ProjectId = project.Id, CurrencyId = currency.Id,
            ContractNo = "CON-001", Title = "Ana Yüklenici Sözleşmesi", ContractAmount = 1000000m,
            StartDate = DateTime.UtcNow.AddMonths(-2), EndDate = DateTime.UtcNow.AddMonths(10), Status = DocumentStatus.Approved,
        }, ct);

        await GetOrAddAsync(_db.ContractParties,
            p => p.ContractId == contract.Id && p.BusinessPartnerId == customer.Id,
            () => new ContractParty
            {
                Id = Guid.NewGuid(), ContractId = contract.Id, BusinessPartnerId = customer.Id, PartyRole = "Customer",
            }, ct);

        var line = await GetOrAddAsync(_db.ContractLines,
            l => l.ContractId == contract.Id,
            () => new ContractLine
            {
                Id = Guid.NewGuid(), ContractId = contract.Id, Description = "Kaba inşaat işleri", Quantity = 1m, UnitPrice = 1000000m,
            }, ct);

        await GetOrAddAsync(_db.ContractAmendments,
            a => a.ContractId == contract.Id,
            () => new ContractAmendment
            {
                Id = Guid.NewGuid(), ContractId = contract.Id, AmendmentNo = "CA-001", AmendmentDate = DateTime.UtcNow.AddDays(-10),
                Description = "Kapsam genişletme", AmountDelta = 50000m,
            }, ct);

        return (contract, line);
    }

    // =====================================================================================
    //  ProgressPayments — hakediş başlığı, satırı, kesintisi
    // =====================================================================================
    private async Task SeedProgressPaymentsAsync(
        Contract contract, ContractLine contractLine, BusinessPartner customer, CancellationToken ct)
    {
        var pp = await GetOrAddAsync(_db.ProgressPayments, p => p.ProgressPaymentNo == "PP-001", () => new ProgressPayment
        {
            Id = Guid.NewGuid(), ContractId = contract.Id, PartnerId = customer.Id, ProgressPaymentNo = "PP-001",
            PaymentPeriodStart = DateTime.UtcNow.AddMonths(-1), PaymentPeriodEnd = DateTime.UtcNow,
            GrossAmount = 200000m, DeductionTotal = 20000m, NetAmount = 180000m, Status = ApprovalRequestStatus.Pending,
        }, ct);

        await GetOrAddAsync(_db.ProgressPaymentLines,
            l => l.ProgressPaymentId == pp.Id,
            () => new ProgressPaymentLine
            {
                Id = Guid.NewGuid(), ProgressPaymentId = pp.Id, ContractLineId = contractLine.Id,
                Description = "Dönem imalatı", Quantity = 0.2m, UnitPrice = 1000000m, Amount = 200000m,
            }, ct);

        await GetOrAddAsync(_db.ProgressPaymentDeductions,
            d => d.ProgressPaymentId == pp.Id,
            () => new ProgressPaymentDeduction
            {
                Id = Guid.NewGuid(), ProgressPaymentId = pp.Id, DeductionType = "Retention", Amount = 20000m, Note = "Teminat kesintisi",
            }, ct);
    }

    // =====================================================================================
    //  Documents — klasör, belge, versiyon, ilişki, erişim yetkisi
    // =====================================================================================
    private async Task SeedDocumentsAsync(Project project, User admin, CancellationToken ct)
    {
        var folder = await GetOrAddAsync(_db.DocumentFolders, f => f.Name == "Proje Belgeleri", () => new DocumentFolder
        {
            Id = Guid.NewGuid(), Name = "Proje Belgeleri",
        }, ct);

        var document = await GetOrAddAsync(_db.Documents,
            d => d.Name == "Sözleşme PDF" && d.DocumentFolderId == folder.Id,
            () => new Document
            {
                Id = Guid.NewGuid(), DocumentFolderId = folder.Id, Name = "Sözleşme PDF", Description = "Ana sözleşme",
                Status = DocumentStatus.Approved, CurrentVersionNo = 1,
            }, ct);

        await GetOrAddAsync(_db.DocumentVersions,
            v => v.DocumentId == document.Id && v.VersionNo == 1,
            () => new DocumentVersion
            {
                Id = Guid.NewGuid(), DocumentId = document.Id, VersionNo = 1, FileName = "sozlesme.pdf",
                FilePath = "/demo/sozlesme.pdf", FileSize = 102400, ContentType = "application/pdf", UploadedAt = DateTime.UtcNow.AddDays(-3),
            }, ct);

        await GetOrAddAsync(_db.DocumentRelations,
            r => r.DocumentId == document.Id && r.RelatedEntityId == project.Id,
            () => new DocumentRelation
            {
                Id = Guid.NewGuid(), DocumentId = document.Id, RelatedModule = "Projects", RelatedEntityType = "Project", RelatedEntityId = project.Id,
            }, ct);

        await GetOrAddAsync(_db.DocumentPermissions,
            p => p.DocumentId == document.Id && p.UserId == admin.Id,
            () => new DocumentPermission
            {
                Id = Guid.NewGuid(), DocumentId = document.Id, UserId = admin.Id, AccessType = "Manage",
            }, ct);
    }

    // =====================================================================================
    //  Workflow — onay koşulu, talep adımı/onaycısı, onay hareketi, yetki devri
    // =====================================================================================
    private async Task SeedWorkflowExtrasAsync(PurchaseOrder purchaseOrder, User admin, User secondUser, CancellationToken ct)
    {
        var version = await (from v in _db.ApprovalDefinitionVersions.IgnoreQueryFilters()
                             join d in _db.ApprovalDefinitions.IgnoreQueryFilters() on v.ApprovalDefinitionId equals d.Id
                             where d.Code == "PurchaseOrderApproval" && v.IsActive
                             select v).FirstOrDefaultAsync(ct);
        if (version is not null)
        {
            await GetOrAddAsync(_db.ApprovalConditions,
                c => c.ApprovalDefinitionVersionId == version.Id && c.FieldName == "Amount",
                () => new ApprovalCondition
                {
                    Id = Guid.NewGuid(), ApprovalDefinitionVersionId = version.Id, FieldName = "Amount",
                    Operator = ConditionOperator.GreaterThanOrEqual, ValueNumber = 1000m,
                }, ct);
        }

        var request = await _db.ApprovalRequests.IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.RelatedModule == "Procurement" && a.RelatedEntityId == purchaseOrder.Id, ct);
        if (request is not null)
        {
            var stepDef = await (from s in _db.ApprovalStepDefinitions.IgnoreQueryFilters()
                                 where s.ApprovalDefinitionVersionId == request.ApprovalDefinitionVersionId && s.StepNo == 1
                                 select s).FirstOrDefaultAsync(ct);
            if (stepDef is not null)
            {
                var requestStep = await GetOrAddAsync(_db.ApprovalRequestSteps,
                    s => s.ApprovalRequestId == request.Id && s.StepNo == 1,
                    () => new ApprovalRequestStep
                    {
                        Id = Guid.NewGuid(), ApprovalRequestId = request.Id, ApprovalStepDefinitionId = stepDef.Id,
                        StepNo = 1, ApprovalMode = ApprovalMode.Sequential, Status = ApprovalStepStatus.Active,
                    }, ct);

                await GetOrAddAsync(_db.ApprovalRequestApprovers,
                    a => a.ApprovalRequestStepId == requestStep.Id && a.UserId == admin.Id,
                    () => new ApprovalRequestApprover
                    {
                        Id = Guid.NewGuid(), ApprovalRequestStepId = requestStep.Id, UserId = admin.Id, Status = ApprovalApproverStatus.Waiting,
                    }, ct);

                await GetOrAddAsync(_db.ApprovalActions,
                    x => x.ApprovalRequestId == request.Id,
                    () => new ApprovalAction
                    {
                        Id = Guid.NewGuid(), ApprovalRequestId = request.Id, ApprovalRequestStepId = requestStep.Id,
                        UserId = admin.Id, ActionType = ApprovalActionType.Return, ActionAt = DateTime.UtcNow.AddHours(-2),
                        Note = "Ek bilgi istendi (demo hareketi)",
                    }, ct);
            }
        }

        if (secondUser.Id != admin.Id)
        {
            await GetOrAddAsync(_db.ApprovalDelegations,
                d => d.DelegatorUserId == admin.Id && d.DelegateUserId == secondUser.Id,
                () => new ApprovalDelegation
                {
                    Id = Guid.NewGuid(), DelegatorUserId = admin.Id, DelegateUserId = secondUser.Id,
                    StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(7), IsActive = true,
                }, ct);
        }
    }

    // =====================================================================================
    //  Notifications — bildirim, alıcı, tercih
    // =====================================================================================
    private async Task SeedNotificationsAsync(Material material, User admin, CancellationToken ct)
    {
        var notification = await GetOrAddAsync(_db.Notifications,
            n => n.NotificationType == "LowStock" && n.RelatedEntityId == material.Id,
            () => new Notification
            {
                Id = Guid.NewGuid(), Title = "Düşük stok uyarısı", Body = "Çimento 50kg stoğu kritik seviyede.",
                NotificationType = "LowStock", RelatedModule = "Inventory", RelatedEntityType = "Material", RelatedEntityId = material.Id,
            }, ct);

        await GetOrAddAsync(_db.NotificationRecipients,
            r => r.NotificationId == notification.Id && r.UserId == admin.Id,
            () => new NotificationRecipient
            {
                Id = Guid.NewGuid(), NotificationId = notification.Id, UserId = admin.Id, IsRead = false,
            }, ct);

        await GetOrAddAsync(_db.NotificationPreferences,
            p => p.UserId == admin.Id && p.NotificationType == "LowStock",
            () => new NotificationPreference
            {
                Id = Guid.NewGuid(), UserId = admin.Id, NotificationType = "LowStock", InAppEnabled = true, EmailEnabled = true,
            }, ct);
    }

    // =====================================================================================
    //  Reporting — rapor tanımı (DashboardWidget zaten kurumsal tohumlamada eklenir)
    // =====================================================================================
    private async Task SeedReportingAsync(CancellationToken ct)
    {
        await GetOrAddAsync(_db.ReportDefinitions, r => r.Code == "RPT-001", () => new ReportDefinition
        {
            Id = Guid.NewGuid(), Code = "RPT-001", Name = "Proje Maliyet Raporu", Module = "Reporting",
            QueryKey = "project-cost-summary", RequiredPermissionCode = "Reporting.ReadAll", IsActive = true,
        }, ct);
    }

    // =====================================================================================
    //  IAM ekleri — doğrudan kullanıcı yetkisi, kullanıcı ayarı, denetim kaydı
    // =====================================================================================
    private async Task SeedDirectUserGrantsAndAuditAsync(User admin, User thirdUser, CancellationToken ct)
    {
        // Doğrudan kullanıcı→yetki ataması: rolü üzerinden gelmeyen ek bir yetki.
        if (!await _db.UserPermissions.AnyAsync(up => up.UserId == thirdUser.Id && up.PermissionCode == "Reporting.Export", ct))
        {
            _db.UserPermissions.Add(new UserPermission { UserId = thirdUser.Id, PermissionCode = "Reporting.Export" });
            await _db.SaveChangesAsync(ct);
        }

        // Kullanıcı tercihi (her kullanıcı için tek satır, UserId ile anahtarlı).
        if (!await _db.UserSettings.AnyAsync(s => s.UserId == admin.Id, ct))
        {
            _db.UserSettings.Add(new UserSetting
            {
                UserId = admin.Id, NotificationSound = true, CallSound = true, DesktopNotifications = true,
                ReadReceipts = true, Theme = "system", UpdatedAt = DateTime.UtcNow,
            });
            await _db.SaveChangesAsync(ct);
        }

        // Denetim kaydı (append-only) — örnek bir başarılı istek.
        if (!await _db.AuditLogs.AnyAsync(a => a.Path == "/seed/demo", ct))
        {
            _db.AuditLogs.Add(new AuditLog
            {
                OccurredAt = DateTime.UtcNow, UserId = admin.Id, UserName = "admin", IpAddress = "127.0.0.1",
                HttpMethod = "GET", Path = "/seed/demo", StatusCode = 200, IsSuccess = true, Source = "Seed",
                CorrelationId = Guid.NewGuid(), DurationMs = 5,
            });
            await _db.SaveChangesAsync(ct);
        }
    }

    // =====================================================================================
    //  Chat — grup, üyeler, birebir + grup mesajı, mesaj tepkisi
    // =====================================================================================
    private async Task SeedChatAsync(User admin, User secondUser, CancellationToken ct)
    {
        var group = await GetOrAddAsync(_db.ChatGroups,
            g => g.Name == "Demo Proje Ekibi" && g.OwnerId == admin.Id,
            () => new ChatGroup { Id = Guid.NewGuid(), Name = "Demo Proje Ekibi", OwnerId = admin.Id }, ct);

        await GetOrAddAsync(_db.ChatGroupMembers,
            m => m.GroupId == group.Id && m.UserId == admin.Id,
            () => new ChatGroupMember
            {
                Id = Guid.NewGuid(), GroupId = group.Id, UserId = admin.Id, Status = ChatGroupMemberStatus.Accepted,
                IsOwner = true, IsAdmin = true,
            }, ct);

        if (secondUser.Id != admin.Id)
        {
            await GetOrAddAsync(_db.ChatGroupMembers,
                m => m.GroupId == group.Id && m.UserId == secondUser.Id,
                () => new ChatGroupMember
                {
                    Id = Guid.NewGuid(), GroupId = group.Id, UserId = secondUser.Id, Status = ChatGroupMemberStatus.Accepted,
                    IsOwner = false, IsAdmin = false, InvitedById = admin.Id,
                }, ct);
        }

        // Grup mesajı.
        await GetOrAddAsync(_db.ChatMessages,
            x => x.GroupId == group.Id && x.Text == "Gruba hoş geldiniz.",
            () => new ChatMessage
            {
                Id = Guid.NewGuid(), SenderId = admin.Id, GroupId = group.Id, Text = "Gruba hoş geldiniz.", IsRead = false,
            }, ct);

        // Birebir mesaj + tepki.
        if (secondUser.Id != admin.Id)
        {
            var directMessage = await GetOrAddAsync(_db.ChatMessages,
                x => x.SenderId == admin.Id && x.RecipientId == secondUser.Id && x.Text == "Merhaba, demo mesajı.",
                () => new ChatMessage
                {
                    Id = Guid.NewGuid(), SenderId = admin.Id, RecipientId = secondUser.Id, Text = "Merhaba, demo mesajı.", IsRead = false,
                }, ct);

            await GetOrAddAsync(_db.ChatMessageReactions,
                r => r.MessageId == directMessage.Id && r.UserId == secondUser.Id,
                () => new ChatMessageReaction
                {
                    Id = Guid.NewGuid(), MessageId = directMessage.Id, UserId = secondUser.Id, Emoji = "👍",
                }, ct);
        }
    }

    #endregion

    #region 08 | DEMO SENARYO | Son 3 ay (90 gun) hacimli operasyonel veri

    /// <summary>Demo verisinin yayıldığı pencere (gün). Son 3 ayı temsil eder.</summary>
    private const int DemoWindowDays = 90;

    private static readonly PurchaseOrderStatus[] DemoPoStatuses =
    {
        PurchaseOrderStatus.Draft, PurchaseOrderStatus.Approved, PurchaseOrderStatus.PartiallyReceived,
        PurchaseOrderStatus.Received, PurchaseOrderStatus.Cancelled,
    };

    private static readonly WorkOrderStatus[] DemoWoStatuses =
    {
        WorkOrderStatus.Draft, WorkOrderStatus.Assigned, WorkOrderStatus.InProgress,
        WorkOrderStatus.OnHold, WorkOrderStatus.Completed, WorkOrderStatus.Closed,
    };

    private static readonly ApprovalRequestStatus[] DemoApprovalStates =
    {
        ApprovalRequestStatus.Draft, ApprovalRequestStatus.Pending, ApprovalRequestStatus.Approved,
        ApprovalRequestStatus.Rejected, ApprovalRequestStatus.Returned, ApprovalRequestStatus.Cancelled,
    };

    /// <summary>Son 90 güne (3 ay) yayılmış, tüm case'leri kapsayan hacimli demo veriyi üretir.</summary>
    private async Task EnsureDemoQuarterDataAsync(CancellationToken ct)
    {
        var admin = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "admin", ct);
        var currency = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "TRY", ct);
        var project = await _db.Projects.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Code == "PRJ-001", ct);
        var supplier = await _db.BusinessPartners.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "SUP-001", ct);
        var material = await _db.Materials.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Code == "MAT-001", ct);
        var workOrderType = await _db.WorkOrderTypes.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Code == "WOT-001", ct);
        var approvalVersion = await _db.ApprovalDefinitionVersions.IgnoreQueryFilters()
            .OrderBy(v => v.VersionNo).FirstOrDefaultAsync(ct);

        if (admin is null || currency is null || project is null || supplier is null ||
            material is null || workOrderType is null)
        {
            _logger.LogWarning("Demo quarter data skipped: one or more anchor records are missing.");
            return;
        }

        // Pencere başlangıcı: bugünden 90 gün öncesi. Tüm operasyonel tarihler bu
        // pencere ile bugün arasına düşer (gelecekteki tarih üretilmez).
        var windowStart = DateTime.UtcNow.Date.AddDays(-(DemoWindowDays - 1));

        await SeedDemoPurchaseOrdersAsync(windowStart, supplier, project, currency, material, ct);
        await SeedDemoWorkOrdersAsync(windowStart, workOrderType, project, ct);
        if (approvalVersion is not null)
        {
            await SeedDemoApprovalsAsync(windowStart, approvalVersion, admin, ct);
        }
        await SeedDemoNotificationsAsync(windowStart, admin, ct);

        _logger.LogInformation("Demo quarter data: high-volume, all-status operational records seeded across {Days} days.", DemoWindowDays);
    }

    // 36 satın alma siparişi — 5 durumun tamamı, çeyreğe yayılı, her birinde satır.
    private async Task SeedDemoPurchaseOrdersAsync(
        DateTime windowStart, BusinessPartner supplier, Project project, Currency currency, Material material, CancellationToken ct)
    {
        for (var i = 1; i <= 36; i++)
        {
            var code = $"PO-D{i:00}";
            var status = DemoPoStatuses[i % DemoPoStatuses.Length];
            var orderDate = windowStart.AddDays((i * 5) % DemoWindowDays);

            var po = await GetOrAddAsync(_db.PurchaseOrders, o => o.OrderNo == code, () => new PurchaseOrder
            {
                Id = Guid.NewGuid(), SupplierId = supplier.Id, ProjectId = project.Id, CurrencyId = currency.Id,
                Status = status, OrderNo = code, OrderDate = orderDate,
            }, ct);

            var qty = 10m + i;
            var received = status switch
            {
                PurchaseOrderStatus.Received => qty,
                PurchaseOrderStatus.PartiallyReceived => Math.Round(qty / 2m, 2),
                _ => 0m,
            };

            await EnsureAsync(_db.PurchaseOrderLines, l => l.PurchaseOrderId == po.Id, () => new PurchaseOrderLine
            {
                Id = Guid.NewGuid(), PurchaseOrderId = po.Id, MaterialId = material.Id,
                Quantity = qty, UnitPrice = 100m + i, CurrencyId = currency.Id, ReceivedQuantity = received,
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 18 iş emri — 6 durumun tamamı, çeyreğe yayılı planlı tarihlerle.
    private async Task SeedDemoWorkOrdersAsync(
        DateTime windowStart, WorkOrderType workOrderType, Project project, CancellationToken ct)
    {
        for (var i = 1; i <= 18; i++)
        {
            var code = $"WO-D{i:00}";
            var status = DemoWoStatuses[i % DemoWoStatuses.Length];
            var start = windowStart.AddDays((i * 5) % DemoWindowDays);

            await GetOrAddAsync(_db.WorkOrders, w => w.WorkOrderNo == code, () => new WorkOrder
            {
                Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id,
                Status = status, WorkOrderNo = code, Title = $"Demo İş Emri {i:00}",
                Description = $"Çeyreklik demo iş emri ({status}).",
                PlannedStart = start, PlannedEnd = start.AddDays(2),
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // Onay talepleri — 6 durumun tamamı + terminal durumlar için ilgili onay hareketi.
    private async Task SeedDemoApprovalsAsync(
        DateTime windowStart, ApprovalDefinitionVersion version, User admin, CancellationToken ct)
    {
        for (var i = 0; i < DemoApprovalStates.Length; i++)
        {
            var state = DemoApprovalStates[i];
            var marker = $"DemoApproval-{state}";

            var request = await GetOrAddAsync(_db.ApprovalRequests,
                r => r.RelatedModule == "DemoQuarter" && r.RelatedEntityType == marker,
                () => new ApprovalRequest
                {
                    Id = Guid.NewGuid(), ApprovalDefinitionVersionId = version.Id,
                    RelatedModule = "DemoQuarter", RelatedEntityType = marker, RelatedEntityId = Guid.NewGuid(),
                    RequestedByUserId = admin.Id, Status = state, CurrentStepNo = 1,
                }, ct);

            var actionType = state switch
            {
                ApprovalRequestStatus.Approved => (ApprovalActionType?)ApprovalActionType.Approve,
                ApprovalRequestStatus.Rejected => ApprovalActionType.Reject,
                ApprovalRequestStatus.Returned => ApprovalActionType.Return,
                ApprovalRequestStatus.Cancelled => ApprovalActionType.Cancel,
                _ => null,
            };

            if (actionType is not null)
            {
                await EnsureAsync(_db.ApprovalActions, a => a.ApprovalRequestId == request.Id, () => new ApprovalAction
                {
                    Id = Guid.NewGuid(), ApprovalRequestId = request.Id, UserId = admin.Id,
                    ActionType = actionType.Value, ActionAt = windowStart.AddDays(i * 15),
                    Note = $"Demo {actionType} hareketi.",
                }, ct);
            }
        }
        await _db.SaveChangesAsync(ct);
    }

    // 24 bildirim — çeyreğe yayılı, okunmuş/okunmamış karışık.
    private async Task SeedDemoNotificationsAsync(DateTime windowStart, User admin, CancellationToken ct)
    {
        for (var i = 1; i <= 24; i++)
        {
            var title = $"DEMO-N{i:00}";
            var read = i % 3 == 0;
            var occurredAt = windowStart.AddDays((i * 7) % DemoWindowDays);

            var notification = await GetOrAddAsync(_db.Notifications, n => n.Title == title, () => new Notification
            {
                Id = Guid.NewGuid(), Title = title, Body = $"Çeyreklik demo bildirim {i:00}.",
                NotificationType = i % 2 == 0 ? "Info" : "Warning", RelatedModule = "DemoQuarter",
            }, ct);

            await EnsureAsync(_db.NotificationRecipients,
                r => r.NotificationId == notification.Id && r.UserId == admin.Id,
                () => new NotificationRecipient
                {
                    Id = Guid.NewGuid(), NotificationId = notification.Id, UserId = admin.Id,
                    IsRead = read, ReadAt = read ? occurredAt : null,
                }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region 10 | DEMO HACIM | Ayni tablolarda 90 gune yayili bagimsiz cok kayit (sistem davranisi)

    private static readonly RequestStatus[] DemoRequestStatuses =
    {
        RequestStatus.Draft, RequestStatus.PendingApproval, RequestStatus.Approved,
        RequestStatus.Rejected, RequestStatus.Ordered, RequestStatus.Closed,
    };

    /// <summary>
    /// Ana is tablolarina 90 gune yayilmis, birbirinden BAGIMSIZ COK kayit ekler; boylece
    /// hacim, trend, yaslandirma (aging), filtreleme, sayfalama ve raporlardaki sistem
    /// davranisi gerceginde oldugu gibi gozlemlenebilir. Tum kayitlar dogal anahtarla
    /// idempotent'tir ve ortak anchor master verilere (proje, tedarikci, musteri, malzeme,
    /// depo, sozlesme, calisan, hesap) FK ile baglidir.
    /// </summary>
    private async Task EnsureDemoVolumeAsync(CancellationToken ct)
    {
        var admin = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "admin", ct);
        var currency = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "TRY", ct);
        var unit = await _db.UnitsOfMeasure.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == "Piece", ct);
        var project = await _db.Projects.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Code == "PRJ-001", ct);
        var supplier = await _db.BusinessPartners.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "SUP-001", ct);
        var customer = await _db.BusinessPartners.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "CUS-001", ct);
        var material = await _db.Materials.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Code == "MAT-001", ct);
        var warehouse = await _db.Warehouses.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Code == "WH-001", ct);
        var workOrder = await _db.WorkOrders.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.WorkOrderNo == "WO-001", ct);
        var employee = await _db.Employees.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Code == "EMP-001", ct);
        var equipment = await _db.EquipmentAssets.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Code == "EQ-001", ct);
        var account = await _db.FinancialAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Code == "FA-001", ct);
        var requestType = await _db.RequestTypes.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Code == "RQT-001", ct);
        var inType = await _db.StockDocumentTypes.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Code == "SDT-IN", ct);
        var outType = await _db.StockDocumentTypes.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Code == "SDT-OUT", ct);
        var folder = await _db.DocumentFolders.IgnoreQueryFilters().FirstOrDefaultAsync(f => f.Name == "Proje Belgeleri", ct);
        var contract = await _db.Contracts.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.ContractNo == "CON-001", ct);

        if (admin is null || currency is null || unit is null || project is null || supplier is null ||
            customer is null || material is null || warehouse is null || workOrder is null || employee is null ||
            equipment is null || account is null || requestType is null || inType is null || outType is null ||
            folder is null || contract is null)
        {
            _logger.LogWarning("Demo volume data skipped: one or more anchor records are missing.");
            return;
        }

        var contractLine = await _db.ContractLines.IgnoreQueryFilters().FirstOrDefaultAsync(l => l.ContractId == contract.Id, ct);
        var windowStart = DateTime.UtcNow.Date.AddDays(-(DemoWindowDays - 1));

        await SeedVolumeStockMovementsAsync(windowStart, inType, outType, warehouse, project, material, unit, currency, ct);
        await SeedVolumePayablesReceivablesAsync(windowStart, supplier, customer, currency, ct);
        await SeedVolumePaymentsCollectionsAsync(windowStart, supplier, customer, currency, account, ct);
        if (contractLine is not null)
        {
            await SeedVolumeProgressPaymentsAsync(windowStart, contract, contractLine, customer, ct);
        }
        await SeedVolumeDailySiteReportsAsync(windowStart, project, workOrder, employee, equipment, material, ct);
        await SeedVolumeTimesheetsAsync(windowStart, employee, project, workOrder, ct);
        await SeedVolumeDocumentsAsync(windowStart, folder, project, ct);
        await SeedVolumeRequestsAsync(windowStart, requestType, project, material, unit, admin, ct);

        _logger.LogInformation("Demo volume data: independent multi-record series seeded across {Days} days.", DemoWindowDays);
    }

    // 30 stok giris (her biri lot+hareket) + her 2.'de bir stok cikis (FIFO dagilimi+ters hareket).
    private async Task SeedVolumeStockMovementsAsync(
        DateTime windowStart, StockDocumentType inType, StockDocumentType outType, Warehouse warehouse,
        Project project, Material material, UnitOfMeasure unit, Currency currency, CancellationToken ct)
    {
        for (var i = 1; i <= 30; i++)
        {
            var date = windowStart.AddDays((i * 3) % DemoWindowDays);
            var inNo = $"SD-VI{i:00}";
            var inDoc = await GetOrAddAsync(_db.StockDocuments, d => d.DocumentNo == inNo, () => new StockDocument
            {
                Id = Guid.NewGuid(), DocumentTypeId = inType.Id, TargetWarehouseId = warehouse.Id, ProjectId = project.Id,
                Status = DocumentStatus.Approved, DocumentNo = inNo, DocumentDate = date, Note = "Demo giris",
            }, ct);
            var inLine = await GetOrAddAsync(_db.StockDocumentLines, l => l.StockDocumentId == inDoc.Id, () => new StockDocumentLine
            {
                Id = Guid.NewGuid(), StockDocumentId = inDoc.Id, MaterialId = material.Id, UnitOfMeasureId = unit.Id,
                Quantity = 50m + i, UnitPrice = 1400m + i, CurrencyId = currency.Id, Note = "Giris",
            }, ct);
            var lotNo = $"LOT-V{i:00}";
            var lot = await GetOrAddAsync(_db.StockLots, l => l.LotNo == lotNo, () => new StockLot
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material.Id, SourceStockDocumentLineId = inLine.Id,
                LotNo = lotNo, InitialQuantity = 50m + i, RemainingQuantity = 40m + i, UnitCost = 1400m + i, ReceivedAt = date,
            }, ct);
            await GetOrAddAsync(_db.StockTransactions, t => t.StockDocumentLineId == inLine.Id, () => new StockTransaction
            {
                Id = Guid.NewGuid(), StockDocumentId = inDoc.Id, StockDocumentLineId = inLine.Id, StockLotId = lot.Id,
                WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = 50m + i, UnitCost = 1400m + i, TransactionDate = date,
            }, ct);

            if (i % 2 == 0)
            {
                var outDate = date.AddDays(1) <= DateTime.UtcNow.Date ? date.AddDays(1) : date;
                var outNo = $"SD-VO{i:00}";
                var outDoc = await GetOrAddAsync(_db.StockDocuments, d => d.DocumentNo == outNo, () => new StockDocument
                {
                    Id = Guid.NewGuid(), DocumentTypeId = outType.Id, SourceWarehouseId = warehouse.Id, ProjectId = project.Id,
                    Status = DocumentStatus.Approved, DocumentNo = outNo, DocumentDate = outDate, Note = "Demo cikis",
                }, ct);
                var outLine = await GetOrAddAsync(_db.StockDocumentLines, l => l.StockDocumentId == outDoc.Id, () => new StockDocumentLine
                {
                    Id = Guid.NewGuid(), StockDocumentId = outDoc.Id, MaterialId = material.Id, UnitOfMeasureId = unit.Id,
                    Quantity = 10m, UnitPrice = 1400m + i, CurrencyId = currency.Id, Note = "Sarf",
                }, ct);
                await GetOrAddAsync(_db.StockIssueAllocations, a => a.StockDocumentLineId == outLine.Id, () => new StockIssueAllocation
                {
                    Id = Guid.NewGuid(), StockDocumentLineId = outLine.Id, StockLotId = lot.Id, Quantity = 10m, UnitCost = 1400m + i,
                }, ct);
                await GetOrAddAsync(_db.StockTransactions, t => t.StockDocumentLineId == outLine.Id, () => new StockTransaction
                {
                    Id = Guid.NewGuid(), StockDocumentId = outDoc.Id, StockDocumentLineId = outLine.Id, StockLotId = lot.Id,
                    WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = -10m, UnitCost = 1400m + i, TransactionDate = outDate,
                }, ct);
            }
        }
        await _db.SaveChangesAsync(ct);
    }

    // 18 borc + 18 alacak — degisken vade ve kalan tutar (acik/kismi/kapali => yaslandirma davranisi).
    private async Task SeedVolumePayablesReceivablesAsync(
        DateTime windowStart, BusinessPartner supplier, BusinessPartner customer, Currency currency, CancellationToken ct)
    {
        for (var i = 1; i <= 18; i++)
        {
            var created = windowStart.AddDays((i * 5) % DemoWindowDays);
            var amount = 5000m + i * 250m;
            var closed = i % 4 == 0;
            var remaining = closed ? 0m : Math.Round(amount * (i % 3 == 0 ? 0.5m : 1m), 2);
            var pMarker = $"DemoPayable-{i:00}";
            await GetOrAddAsync(_db.Payables, p => p.RelatedModule == "DemoVolume" && p.RelatedEntityType == pMarker, () => new Payable
            {
                Id = Guid.NewGuid(), PartnerId = supplier.Id, CurrencyId = currency.Id, Amount = amount, RemainingAmount = remaining,
                DueDate = created.AddDays(30), RelatedModule = "DemoVolume", RelatedEntityType = pMarker, IsClosed = closed,
            }, ct);

            var rAmount = 8000m + i * 400m;
            var rClosed = i % 5 == 0;
            var rRemaining = rClosed ? 0m : Math.Round(rAmount * (i % 2 == 0 ? 0.7m : 1m), 2);
            var rMarker = $"DemoReceivable-{i:00}";
            await GetOrAddAsync(_db.Receivables, r => r.RelatedModule == "DemoVolume" && r.RelatedEntityType == rMarker, () => new Receivable
            {
                Id = Guid.NewGuid(), PartnerId = customer.Id, CurrencyId = currency.Id, Amount = rAmount, RemainingAmount = rRemaining,
                DueDate = created.AddDays(45), RelatedModule = "DemoVolume", RelatedEntityType = rMarker, IsClosed = rClosed,
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 12 odeme + 12 tahsilat — cesitli durumlarla, ceyrege yayili.
    private async Task SeedVolumePaymentsCollectionsAsync(
        DateTime windowStart, BusinessPartner supplier, BusinessPartner customer, Currency currency,
        FinancialAccount account, CancellationToken ct)
    {
        for (var i = 1; i <= 12; i++)
        {
            var date = windowStart.AddDays((i * 7) % DemoWindowDays);
            var payNo = $"PAY-V{i:00}";
            await GetOrAddAsync(_db.Payments, p => p.PaymentNo == payNo, () => new Payment
            {
                Id = Guid.NewGuid(), PartnerId = supplier.Id, CurrencyId = currency.Id, FinancialAccountId = account.Id,
                Amount = 1500m + i * 100m, PaymentDate = date, PaymentNo = payNo,
                Status = i % 3 == 0 ? ApprovalRequestStatus.Pending : ApprovalRequestStatus.Approved,
            }, ct);

            var colNo = $"COL-V{i:00}";
            await GetOrAddAsync(_db.Collections, c => c.CollectionNo == colNo, () => new Collection
            {
                Id = Guid.NewGuid(), PartnerId = customer.Id, CurrencyId = currency.Id, FinancialAccountId = account.Id,
                Amount = 3000m + i * 200m, CollectionDate = date, CollectionNo = colNo,
                Status = i % 4 == 0 ? ApprovalRequestStatus.Pending : ApprovalRequestStatus.Approved,
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 9 hakedis donemi — her biri satir + kesinti ile (aylik ilerleme davranisi).
    private async Task SeedVolumeProgressPaymentsAsync(
        DateTime windowStart, Contract contract, ContractLine contractLine, BusinessPartner customer, CancellationToken ct)
    {
        for (var i = 1; i <= 9; i++)
        {
            var periodStart = windowStart.AddDays((i - 1) * 10);
            var periodEnd = periodStart.AddDays(9);
            var no = $"PP-V{i:00}";
            var gross = 100000m + i * 15000m;
            var deduction = Math.Round(gross * 0.1m, 2);
            var pp = await GetOrAddAsync(_db.ProgressPayments, p => p.ProgressPaymentNo == no, () => new ProgressPayment
            {
                Id = Guid.NewGuid(), ContractId = contract.Id, PartnerId = customer.Id, ProgressPaymentNo = no,
                PaymentPeriodStart = periodStart, PaymentPeriodEnd = periodEnd,
                GrossAmount = gross, DeductionTotal = deduction, NetAmount = gross - deduction,
                Status = i % 3 == 0 ? ApprovalRequestStatus.Pending : ApprovalRequestStatus.Approved,
            }, ct);
            await GetOrAddAsync(_db.ProgressPaymentLines, l => l.ProgressPaymentId == pp.Id, () => new ProgressPaymentLine
            {
                Id = Guid.NewGuid(), ProgressPaymentId = pp.Id, ContractLineId = contractLine.Id,
                Description = $"Donem {i} imalati", Quantity = 0.1m, UnitPrice = contractLine.UnitPrice, Amount = gross,
            }, ct);
            await GetOrAddAsync(_db.ProgressPaymentDeductions, d => d.ProgressPaymentId == pp.Id, () => new ProgressPaymentDeduction
            {
                Id = Guid.NewGuid(), ProgressPaymentId = pp.Id, DeductionType = "Retention", Amount = deduction, Note = "Teminat kesintisi",
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 40 gunluk saha raporu (+isci/ekipman/malzeme) — gunluk saha faaliyeti.
    private async Task SeedVolumeDailySiteReportsAsync(
        DateTime windowStart, Project project, WorkOrder workOrder, Employee employee, EquipmentAsset equipment,
        Material material, CancellationToken ct)
    {
        for (var i = 1; i <= 40; i++)
        {
            var date = windowStart.AddDays((i * 2) % DemoWindowDays);
            var no = $"DSR-V{i:000}";
            var report = await GetOrAddAsync(_db.DailySiteReports, r => r.ReportNo == no, () => new DailySiteReport
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, WorkOrderId = workOrder.Id, ReportNo = no,
                ReportDate = date, Weather = i % 3 == 0 ? "Yagmurlu" : "Acik", Notes = $"Gun {i} faaliyet", Status = DocumentStatus.Approved,
            }, ct);
            await GetOrAddAsync(_db.DailySiteReportWorkers, w => w.DailySiteReportId == report.Id, () => new DailySiteReportWorker
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, EmployeeId = employee.Id, HoursWorked = 8m, Note = "Tam gun",
            }, ct);
            await GetOrAddAsync(_db.DailySiteReportEquipments, e => e.DailySiteReportId == report.Id, () => new DailySiteReportEquipment
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, EquipmentAssetId = equipment.Id, Hours = 6m,
            }, ct);
            await GetOrAddAsync(_db.DailySiteReportMaterials, m => m.DailySiteReportId == report.Id, () => new DailySiteReportMaterial
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, MaterialId = material.Id, Quantity = 5m + i % 10,
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 12 haftalik puantaj (+satir) — donemsel iscilik maliyeti.
    private async Task SeedVolumeTimesheetsAsync(
        DateTime windowStart, Employee employee, Project project, WorkOrder workOrder, CancellationToken ct)
    {
        for (var i = 1; i <= 12; i++)
        {
            var periodStart = windowStart.AddDays((i - 1) * 7);
            var periodEnd = periodStart.AddDays(6);
            var no = $"TS-V{i:00}";
            var ts = await GetOrAddAsync(_db.Timesheets, t => t.TimesheetNo == no, () => new Timesheet
            {
                Id = Guid.NewGuid(), TimesheetNo = no, PeriodStart = periodStart, PeriodEnd = periodEnd,
                Status = i % 3 == 0 ? ApprovalRequestStatus.Pending : ApprovalRequestStatus.Approved,
            }, ct);
            await GetOrAddAsync(_db.TimesheetLines, l => l.TimesheetId == ts.Id, () => new TimesheetLine
            {
                Id = Guid.NewGuid(), TimesheetId = ts.Id, EmployeeId = employee.Id, ProjectId = project.Id,
                WorkOrderId = workOrder.Id, WorkDate = periodEnd, NormalHours = 40m, OvertimeHours = (decimal)(i % 5), HourlyCost = 150m,
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 15 belge — her biri 1..3 versiyon + proje iliskisi (versiyon gecmisi davranisi).
    private async Task SeedVolumeDocumentsAsync(
        DateTime windowStart, DocumentFolder folder, Project project, CancellationToken ct)
    {
        for (var i = 1; i <= 15; i++)
        {
            var created = windowStart.AddDays((i * 6) % DemoWindowDays);
            var name = $"Demo Belge {i:00}";
            var versionCount = (i % 3) + 1;
            var doc = await GetOrAddAsync(_db.Documents, d => d.Name == name && d.DocumentFolderId == folder.Id, () => new Document
            {
                Id = Guid.NewGuid(), DocumentFolderId = folder.Id, Name = name, Description = $"Demo belge {i}",
                Status = DocumentStatus.Approved, CurrentVersionNo = versionCount,
            }, ct);
            for (var v = 1; v <= versionCount; v++)
            {
                var versionNo = v;
                await GetOrAddAsync(_db.DocumentVersions, dv => dv.DocumentId == doc.Id && dv.VersionNo == versionNo, () => new DocumentVersion
                {
                    Id = Guid.NewGuid(), DocumentId = doc.Id, VersionNo = versionNo, FileName = $"belge-{i:00}-v{versionNo}.pdf",
                    FilePath = $"/demo/belge-{i:00}-v{versionNo}.pdf", FileSize = 50000 + versionNo * 1000,
                    ContentType = "application/pdf", UploadedAt = created.AddDays(versionNo - 1),
                }, ct);
            }
            await GetOrAddAsync(_db.DocumentRelations, r => r.DocumentId == doc.Id && r.RelatedEntityId == project.Id, () => new DocumentRelation
            {
                Id = Guid.NewGuid(), DocumentId = doc.Id, RelatedModule = "Projects", RelatedEntityType = "Project", RelatedEntityId = project.Id,
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 18 talep (+satir) — 6 durumun tamamina yayili (talep yasam dongusu davranisi).
    private async Task SeedVolumeRequestsAsync(
        DateTime windowStart, RequestType requestType, Project project, Material material, UnitOfMeasure unit,
        User admin, CancellationToken ct)
    {
        for (var i = 1; i <= 18; i++)
        {
            var date = windowStart.AddDays((i * 5) % DemoWindowDays);
            var no = $"REQ-V{i:00}";
            var status = DemoRequestStatuses[i % DemoRequestStatuses.Length];
            var request = await GetOrAddAsync(_db.Requests, r => r.RequestNo == no, () => new Request
            {
                Id = Guid.NewGuid(), RequestTypeId = requestType.Id, ProjectId = project.Id, RequestedByUserId = admin.Id,
                Status = status, RequestNo = no, RequestDate = date, Description = $"Saha malzeme talebi {i}",
            }, ct);
            await GetOrAddAsync(_db.RequestLines, l => l.RequestId == request.Id, () => new RequestLine
            {
                Id = Guid.NewGuid(), RequestId = request.Id, MaterialId = material.Id, Quantity = 20m + i,
                UnitOfMeasureId = unit.Id, Note = "Demo talep kalemi",
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region 09 | DOGRULAMA (per-table coverage)

    // DbContext.Set<TEntity>() (parametresiz, generic) — entity başına IQueryable üretmek için.
    private static readonly MethodInfo SetMethodDefinition = typeof(DbContext)
        .GetMethods()
        .Single(m => m.Name == nameof(DbContext.Set)
            && m.IsGenericMethodDefinition
            && m.GetParameters().Length == 0);

    // EntityFrameworkQueryableExtensions.CountAsync<TSource>(IQueryable<TSource>, CancellationToken).
    private static readonly MethodInfo CountAsyncMethodDefinition = typeof(EntityFrameworkQueryableExtensions)
        .GetMethods()
        .Single(m => m.Name == nameof(EntityFrameworkQueryableExtensions.CountAsync)
            && m.GetParameters().Length == 2);

    /// <summary>
    /// Her tabloyu sayar; kapsama özetini ("{populated}/{total} tablo dolu") loglar ve
    /// satırı olmayan tabloları açık şekilde bildirir. Doğrulama amaçlıdır; hata fırlatmaz.
    /// </summary>
    private async Task VerifySeedCoverageAsync(CancellationToken ct)
    {
        // Gerçek tablosu olan entity'ler: owned/sahip türleri ve örtük (shared-type) join
        // tablolarını dışla; bunlar ana entity üzerinden sayılır.
        var entityClrTypes = _db.Model.GetEntityTypes()
            .Where(t => !t.IsOwned()
                && t.ClrType != typeof(Dictionary<string, object>)
                && t.GetTableName() is not null)
            .Select(t => t.ClrType)
            .Distinct()
            .OrderBy(t => t.Name)
            .ToList();

        var populated = 0;
        var empty = new List<string>();

        foreach (var clrType in entityClrTypes)
        {
            int count;
            try
            {
                var query = SetMethodDefinition.MakeGenericMethod(clrType).Invoke(_db, null)!;
                var countTask = (Task<int>)CountAsyncMethodDefinition
                    .MakeGenericMethod(clrType)
                    .Invoke(null, new object?[] { query, ct })!;
                count = await countTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Seed verification: count failed for {Entity}.", clrType.Name);
                continue;
            }

            if (count > 0) populated++;
            else empty.Add(clrType.Name);
        }

        _logger.LogInformation(
            "Seed verification: {Populated}/{Total} table(s) populated.",
            populated, entityClrTypes.Count);

        if (empty.Count > 0)
        {
            _logger.LogWarning(
                "Seed verification: {Count} table(s) have no rows: {Tables}",
                empty.Count, string.Join(", ", empty));
        }
        else
        {
            _logger.LogInformation("Seed verification: every table is populated.");
        }
    }

    #endregion

}

#region HOST UZANTISI | RunSystemSeedingAsync (uygulama acilisinda cagrilir)

public static class SystemSeederExtensions
{
    public static async Task RunSystemSeedingAsync(this IHost host, CancellationToken ct = default)
    {
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SystemSeeder>>();
        try
        {
            var seeder = scope.ServiceProvider.GetRequiredService<SystemSeeder>();
            await seeder.SeedAllAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System seeding failed; aborting startup.");
            throw;
        }
    }
}

#endregion
