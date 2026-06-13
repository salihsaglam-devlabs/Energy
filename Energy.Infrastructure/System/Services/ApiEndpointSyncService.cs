using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Identity.Permissions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.System.Services;

/// <summary>
/// Başlangıçta <see cref="ApiDescription"/> taranır ve uygulamanın sunduğu her
/// (metot, yol) ikilisi kaydedilir. Tohumlayıcının <see cref="DefaultEndpointPermissionMap"/>
/// içinden tanıdığı satırlar, eşleşen yetkiyle ETKİN olarak eklenir; böylece sistem
/// kutudan çıktığı gibi kullanılabilir. Bilinmeyen rotalar yetkisiz ve pasif kalır —
/// bir yönetici gözden geçirene kadar varsayılan REDDET korunur. Zaten var olan
/// satırların üzerine asla yazılmaz; böylece arayüzde yapılan elle düzenlemeler her
/// yeniden başlatmada korunur.
/// </summary>
public sealed class ApiEndpointSyncService
{
    /// <summary>
    /// <c>"Controller.Action"</c> anahtarına göre (büyük/küçük harfe duyarsız)
    /// düzenlenmiş kural haritası. Değer, gereken yetki kodudur; <c>null</c> ise
    /// rotayı herkese açık olarak işaretler (etkin, yetki gerekmez — ör. login, "menüm").
    /// </summary>
    private static readonly IReadOnlyDictionary<string, string?> DefaultEndpointPermissionMap =
        new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            // Auth — login anonimdir; satır yönetici arayüzünde görünürlük için vardır.
            ["Auth.Login"] = null,

            // Ana sayfa / Gösterge panosu
            ["Home.GetDashboard"] = PermissionCatalog.DashboardRead,

            // Kullanıcılar
            ["Users.GetAll"]         = PermissionCatalog.UserReadAll,
            ["Users.GetById"]        = PermissionCatalog.UserRead,
            ["Users.Create"]         = PermissionCatalog.UserCreate,
            ["Users.Update"]         = PermissionCatalog.UserUpdate,
            ["Users.Delete"]         = PermissionCatalog.UserDelete,
            ["Users.ChangePassword"] = PermissionCatalog.UserUpdate,
            ["Users.GetProfileImage"] = PermissionCatalog.ProfileRead,
            ["Users.SetProfileImage"] = PermissionCatalog.ProfileUpdate,
            ["Users.RemoveProfileImage"] = PermissionCatalog.ProfileUpdate,
            ["Users.GetAccess"]      = PermissionCatalog.UserReadAll,
            ["Users.SetAccess"]      = PermissionCatalog.UserUpdate,

            // Roller
            ["Roles.GetAll"]         = PermissionCatalog.RoleReadAll,
            ["Roles.GetById"]        = PermissionCatalog.RoleRead,
            ["Roles.Create"]         = PermissionCatalog.RoleCreate,
            ["Roles.Update"]         = PermissionCatalog.RoleUpdate,
            ["Roles.Delete"]         = PermissionCatalog.RoleDelete,
            ["Roles.SetPermissions"] = PermissionCatalog.RoleUpdate,

            // Yetkiler (arayüzden salt okunur)
            ["Permissions.GetAll"]    = PermissionCatalog.PermissionReadAll,
            ["Permissions.GetByCode"] = PermissionCatalog.PermissionRead,

            // Menüler — "me" geçerli kullanıcının ağacını döndürür, oturum açan herkes için çalışmalıdır.
            ["Menus.GetAll"]    = PermissionCatalog.MenuReadAll,
            ["Menus.GetById"]   = PermissionCatalog.MenuRead,
            ["Menus.GetMyMenu"] = null,
            ["Menus.Create"]    = PermissionCatalog.MenuCreate,
            ["Menus.Update"]    = PermissionCatalog.MenuUpdate,
            ["Menus.Delete"]    = PermissionCatalog.MenuDelete,

            // API uç noktaları
            ["ApiEndpoints.GetAll"]  = PermissionCatalog.ApiAccessReadAll,
            ["ApiEndpoints.GetById"] = PermissionCatalog.ApiAccessRead,
            ["ApiEndpoints.Create"]  = PermissionCatalog.ApiAccessCreate,
            ["ApiEndpoints.Update"]  = PermissionCatalog.ApiAccessUpdate,
            ["ApiEndpoints.Delete"]  = PermissionCatalog.ApiAccessDelete,

            // Yerelleştirme
            ["Localization.GetAll"]   = PermissionCatalog.LocalizationReadAll,
            ["Localization.GetByKey"] = PermissionCatalog.LocalizationRead,
            ["Localization.Upsert"]   = PermissionCatalog.LocalizationUpdate,
            ["Localization.Delete"]   = PermissionCatalog.LocalizationDelete,

            // Tohumlama — System.Seed ile korunan yüksek ayrıcalıklı bakım işlemleri.
            ["Seed.SeedAll"]                  = PermissionCatalog.SystemSeed,
            ["Seed.SeedLocalization"]         = PermissionCatalog.SystemSeed,
            ["Seed.SeedLocalizationFromResx"] = PermissionCatalog.SystemSeed,

            // Denetim günlükleri
            ["AuditLogs.Query"]   = PermissionCatalog.LogReadAll,
            ["AuditLogs.GetById"] = PermissionCatalog.LogRead,
            // Ingest, üst katmanlar (Web) tarafından kendi istek günlüklerini kaydetmek
            // için kullanılır; kimliği doğrulanmış her kullanıcı kendi gezinme kaydını gönderebilir.
            ["AuditLogs.Ingest"]  = null,

            // Sohbet — kimliği doğrulanmış her kullanıcı işbirliği yapar; varsayılan yetki olarak gelir.
            ["Chat.GetContacts"]     = PermissionCatalog.ChatUse,
            ["Chat.GetConversation"] = PermissionCatalog.ChatUse,
            ["Chat.Send"]            = PermissionCatalog.ChatUse,
            ["Chat.MarkRead"]        = PermissionCatalog.ChatUse,
            ["Chat.UnreadCount"]     = PermissionCatalog.ChatUse,
            ["Chat.GetAttachment"]   = PermissionCatalog.ChatUse,
            ["Chat.GetUserAvatar"]   = PermissionCatalog.ChatUse,
            // Sohbet grupları
            ["Chat.GetGroups"]            = PermissionCatalog.ChatUse,
            ["Chat.GetGroupInvites"]      = PermissionCatalog.ChatUse,
            ["Chat.CreateGroup"]          = PermissionCatalog.ChatUse,
            ["Chat.InviteToGroup"]        = PermissionCatalog.ChatUse,
            ["Chat.RespondInvite"]        = PermissionCatalog.ChatUse,
            ["Chat.GetGroupMembers"]      = PermissionCatalog.ChatUse,
            ["Chat.GetGroupMemberIds"]    = PermissionCatalog.ChatUse,
            ["Chat.GetGroupConversation"] = PermissionCatalog.ChatUse,
            // Sohbet grubu yönetimi (serviste sahip/yönetici tarafından grup bazında yetkilendirilir)
            ["Chat.DeleteGroup"]          = PermissionCatalog.ChatUse,
            ["Chat.RemoveMember"]         = PermissionCatalog.ChatUse,
            ["Chat.SetGroupAdmin"]        = PermissionCatalog.ChatUse,
            // Sohbet mesajı eylemleri
            ["Chat.DeleteMessage"]        = PermissionCatalog.ChatUse,
            ["Chat.Forward"]              = PermissionCatalog.ChatUse,
            ["Chat.React"]                = PermissionCatalog.ChatUse,

            // Self servis kullanıcı ayarları — kimliği doğrulanmış her kullanıcı için varsayılan yetki.
            // Hem okuma hem güncelleme aynı self servis yetkisine (UserSettingsRead) bağlıdır:
            // ayarlar ekranı bu yetkiyle korunduğundan, onu görebilen kullanıcı kendi
            // tercihlerini KAYDEDEBİLMELİDİR. Ayrı bir "update" yetkisi gerektirmek, kullanıcının
            // ekranı açıp kaydederken 403 almasına yol açan kırılgan bir duruma neden oluyordu.
            ["Settings.GetMine"]    = PermissionCatalog.UserSettingsRead,
            ["Settings.UpdateMine"] = PermissionCatalog.UserSettingsRead,
        };

    /// <summary>
    /// Varsayılan haritada kullanılan tüm (null olmayan) yetki kodlarının kümesi.
    /// Mevcut bir uç noktanın yetki kodunun, önceki bir sürümün senkronizasyonu
    /// tarafından mı atandığını (yani sistem-yönetimli olduğunu) yoksa yöneticinin
    /// özel seçimi mi olduğunu ayırt etmek için kullanılır.
    /// </summary>
    private static readonly IReadOnlySet<string> DefaultMappedPermissionCodes =
        DefaultEndpointPermissionMap.Values
            .Where(value => !string.IsNullOrEmpty(value))
            .Select(value => value!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private readonly AppDbContext _db;
    private readonly IApiDescriptionGroupCollectionProvider _descriptions;
    private readonly ILogger<ApiEndpointSyncService> _logger;

    /// <summary>Bağımlılıkları (veritabanı, API tanımları sağlayıcısı, günlükleyici) enjekte eder.</summary>
    public ApiEndpointSyncService(
        AppDbContext db,
        IApiDescriptionGroupCollectionProvider descriptions,
        ILogger<ApiEndpointSyncService> logger)
    {
        _db = db;
        _descriptions = descriptions;
        _logger = logger;
    }

    /// <summary>Keşfedilen tüm uç noktaları veritabanıyla senkronize eder (yalnızca eksikleri ekler).</summary>
    public async Task SyncAsync(CancellationToken ct = default)
    {
        var discovered = _descriptions.ApiDescriptionGroups.Items
            .SelectMany(group => group.Items)
            .Where(d => !string.IsNullOrWhiteSpace(d.RelativePath))
            .Select(d => new
            {
                Method = (d.HttpMethod ?? "GET").ToUpperInvariant(),
                Path = "/" + d.RelativePath!.TrimStart('/'),
                Controller = RouteValue(d, "controller"),
                Action = RouteValue(d, "action")
            })
            .DistinctBy(x => (x.Method, x.Path))
            .ToList();

        var existing = await _db.ApiEndpoints.ToListAsync(ct);
        var existingByKey = existing.ToDictionary(
            e => (e.HttpMethod.ToUpperInvariant(), e.Path),
            e => e);

        var added = 0;
        var activated = 0;
        var reconciled = 0;

        foreach (var d in discovered)
        {
            var convention = $"{d.Controller}.{d.Action}";
            var hasDefault = DefaultEndpointPermissionMap.TryGetValue(convention, out var defaultPermission);

            if (existingByKey.TryGetValue((d.Method, d.Path), out var row))
            {
                // Sezgisel: yalnızca önceki senkronizasyonun YETKİSİZ ve PASİF olarak
                // eklediği — yani yöneticinin hiç dokunmadığı — satırları otomatik yapılandır.
                if (hasDefault && !row.IsActive && row.RequiredPermissionCode is null)
                {
                    row.IsActive = true;
                    row.RequiredPermissionCode = defaultPermission;
                    activated += 1;
                }
                // Sistem-yönetimli bir uç noktanın yetki kodu, önceki bir sürümün varsayılan
                // haritasından kaldıysa kaynak-doğruluk (koddaki varsayılan) ile yeniden
                // hizala. GÜVENLİ koşul: mevcut kod hâlâ bilinen bir varsayılan harita
                // değeri olmalı; böylece yöneticinin arayüzden seçtiği ÖZEL (haritada
                // olmayan) yetkilere asla dokunulmaz. Bu, ör. "Settings.UpdateMine"
                // eski UserSettings.Update'ten yeni UserSettings.Read'e taşındığında
                // mevcut veritabanlarının yeniden tohumlamada kendiliğinden düzelmesini sağlar.
                else if (hasDefault
                         && defaultPermission is not null
                         && row.IsActive
                         && row.RequiredPermissionCode is { } current
                         && !string.Equals(current, defaultPermission, StringComparison.OrdinalIgnoreCase)
                         && DefaultMappedPermissionCodes.Contains(current))
                {
                    row.RequiredPermissionCode = defaultPermission;
                    reconciled += 1;
                }
                continue;
            }

            _db.ApiEndpoints.Add(new ApiEndpoint
            {
                Id = Guid.NewGuid(),
                Name = convention,
                Path = d.Path,
                HttpMethod = d.Method,
                IsActive = hasDefault,
                RequiredPermissionCode = hasDefault ? defaultPermission : null
            });
            added += 1;
        }

        if (added > 0 || activated > 0 || reconciled > 0)
        {
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation(
                "ApiEndpoint sync: {Added} new endpoint(s), {Activated} auto-activated, {Reconciled} permission(s) realigned from defaults.",
                added, activated, reconciled);
        }
    }

    /// <summary>Bir eylem tanımındaki rota değerini (controller/action) güvenli şekilde okur.</summary>
    private static string RouteValue(ApiDescription d, string key)
        => d.ActionDescriptor.RouteValues.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value)
            ? value
            : "Unknown";
}
