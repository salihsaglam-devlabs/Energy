using Energy.Domain.Modules.IAM;
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
    private static readonly IReadOnlyDictionary<string, string?> DefaultEndpointPermissionMap = BuildDefaultMap();

    private static IReadOnlyDictionary<string, string?> BuildDefaultMap()
    {
        var map = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            // Auth — login anonimdir; satır yönetici arayüzünde görünürlük için vardır.
            ["Auth.Login"] = null,

            // Ana sayfa / Gösterge panosu
            ["Home.GetDashboard"] = PermissionCatalog.DashboardRead,
            ["Home.EnterpriseMetrics"] = PermissionCatalog.DashboardRead,

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

        // Kurumsal modül CRUD denetleyicileri: "<Module>.<Action>" kuralını standart
        // CRUD permission'larına eşle; böylece 20 iş modülü kutudan çıktığı gibi korunur.
        foreach (var module in PermissionCatalog.CrudModules)
        {
            map[$"{module}.GetAll"] = $"{module}.{PermissionActions.ReadAll}";
            map[$"{module}.GetById"] = $"{module}.{PermissionActions.Read}";
            map[$"{module}.Create"] = $"{module}.{PermissionActions.Create}";
            map[$"{module}.Update"] = $"{module}.{PermissionActions.Update}";
            map[$"{module}.Delete"] = $"{module}.{PermissionActions.Delete}";
        }

        // Ana-detay (master-detail) alt-koleksiyon uç noktaları. Her alt-koleksiyon,
        // ana modülünün ReadAll yetkisiyle korunur (ModuleDetails.<Action> kuralı).
        var readAll = PermissionActions.ReadAll;
        map["ModuleDetails.RequestLines"]               = $"Requests.{readAll}";
        map["ModuleDetails.PurchaseOrderLines"]         = $"Procurement.{readAll}";
        map["ModuleDetails.WorkOrderAssignments"]       = $"Operations.{readAll}";
        map["ModuleDetails.WorkOrderMaterialPlans"]     = $"Operations.{readAll}";
        map["ModuleDetails.WorkOrderChecklists"]        = $"Operations.{readAll}";
        map["ModuleDetails.WorkOrderStatusHistories"]   = $"Operations.{readAll}";
        map["ModuleDetails.DailySiteReportWorkers"]     = $"FieldOperations.{readAll}";
        map["ModuleDetails.DailySiteReportEquipments"]  = $"FieldOperations.{readAll}";
        map["ModuleDetails.DailySiteReportMaterials"]   = $"FieldOperations.{readAll}";
        map["ModuleDetails.TimesheetLines"]             = $"HR.{readAll}";
        map["ModuleDetails.EquipmentAssignments"]       = $"Assets.{readAll}";
        map["ModuleDetails.EquipmentMaintenances"]      = $"Assets.{readAll}";
        map["ModuleDetails.FinancialTransactionLines"]  = $"Finance.{readAll}";
        map["ModuleDetails.BudgetLines"]                = $"Budget.{readAll}";
        map["ModuleDetails.ContractLines"]              = $"Contracts.{readAll}";
        map["ModuleDetails.ContractParties"]            = $"Contracts.{readAll}";
        map["ModuleDetails.ContractAmendments"]         = $"Contracts.{readAll}";
        map["ModuleDetails.ProgressPaymentLines"]       = $"ProgressPayments.{readAll}";
        map["ModuleDetails.ProgressPaymentDeductions"]  = $"ProgressPayments.{readAll}";
        map["ModuleDetails.MaterialAttributeValues"]    = $"Catalog.{readAll}";
        map["ModuleDetails.MaterialUnitConversions"]    = $"Catalog.{readAll}";
        map["ModuleDetails.WarehouseLocations"]         = $"Inventory.{readAll}";

        // Alt-koleksiyon yazma (CRUD) uç noktaları: her satır koleksiyonu kendi ana modülünün
        // Create/Update/Delete yetkisiyle korunur (ModuleDetails.<Create|Update|Delete><Suffix>).
        // Denetim/iz niteliğindeki koleksiyonlar (ör. iş emri durum geçmişi) yazma sunmaz.
        var create = PermissionActions.Create;
        var update = PermissionActions.Update;
        var delete = PermissionActions.Delete;
        var detailWrites = new (string Suffix, string Module)[]
        {
            ("RequestLine", "Requests"),
            ("PurchaseOrderLine", "Procurement"),
            ("WorkOrderAssignment", "Operations"),
            ("WorkOrderMaterialPlan", "Operations"),
            ("WorkOrderChecklist", "Operations"),
            ("DailySiteReportWorker", "FieldOperations"),
            ("DailySiteReportEquipment", "FieldOperations"),
            ("DailySiteReportMaterial", "FieldOperations"),
            ("TimesheetLine", "HR"),
            ("EquipmentAssignment", "Assets"),
            ("EquipmentMaintenance", "Assets"),
            ("FinancialTransactionLine", "Finance"),
            ("BudgetLine", "Budget"),
            ("ContractLine", "Contracts"),
            ("ContractParty", "Contracts"),
            ("ContractAmendment", "Contracts"),
            ("ProgressPaymentLine", "ProgressPayments"),
            ("ProgressPaymentDeduction", "ProgressPayments"),
            ("MaterialAttributeValue", "Catalog"),
            ("MaterialUnitConversion", "Catalog"),
            ("WarehouseLocation", "Inventory"),
        };
        foreach (var (suffix, module) in detailWrites)
        {
            map[$"ModuleDetails.Create{suffix}"] = $"{module}.{create}";
            map[$"ModuleDetails.Update{suffix}"] = $"{module}.{update}";
            map[$"ModuleDetails.Delete{suffix}"] = $"{module}.{delete}";
        }

        // Workflow (onay) motoru eylemleri.
        map["WorkflowActions.Start"] = "Workflow.Create";
        map["WorkflowActions.Approve"] = "Workflow.Approve";
        map["WorkflowActions.Reject"] = "Workflow.Reject";
        map["WorkflowActions.Return"] = "Workflow.Return";
        map["WorkflowActions.Cancel"] = "Workflow.Update";
        map["WorkflowActions.MyPending"] = "Workflow.Read";

        // Standart süreç rotası: onay süreci ekranı uç noktaları.
        map["ApprovalProcess.MyPending"] = "Workflow.Read";
        map["ApprovalProcess.Approve"] = "Workflow.Approve";
        map["ApprovalProcess.Reject"] = "Workflow.Reject";
        map["ApprovalProcess.Cancel"] = "Workflow.Update";

        // Standart süreç rotası: stok ve mal kabul süreç ekranı uç noktaları.
        map["StockIssueProcess.Execute"] = "Inventory.Approve";
        map["StockTransferProcess.Execute"] = "Inventory.Transfer";
        map["GoodsReceiptProcess.Execute"] = "Procurement.Approve";

        // Standart süreç rotası: Finance süreç ekranı uç noktaları.
        map["TimesheetCostProcess.Execute"] = "Finance.Create";
        map["ProgressPaymentPostingProcess.Execute"] = "Finance.Create";

        // Ödeme tahsis süreci.
        map["PaymentAllocationProcess.Execute"] = "Finance.Update";

        // Belge dosya/versiyon yönetimi uç noktaları.
        map["DocumentFiles.Upload"] = "Documents.Upload";
        map["DocumentFiles.Versions"] = "Documents.Read";
        map["DocumentFiles.Download"] = "Documents.Download";

        // Inventory iş kuralı eylemleri.
        map["InventoryActions.StockIn"] = "Inventory.Approve";
        map["InventoryActions.StockOut"] = "Inventory.Approve";
        map["InventoryActions.Transfer"] = "Inventory.Transfer";
        map["InventoryActions.Count"] = "Inventory.Count";
        map["InventoryActions.Rebuild"] = "Inventory.Reverse";
        map["InventoryActions.Reverse"] = "Inventory.Reverse";

        // Operations iş kuralı eylemleri.
        map["OperationsActions.Close"] = "Operations.Update";
        map["OperationsActions.Reopen"] = "Operations.Update";
        map["OperationsActions.ChangeStatus"] = "Operations.Update";

        // Catalog iş kuralı eylemleri.
        map["CatalogActions.Validate"] = "Catalog.Read";
        map["CatalogActions.Activate"] = "Catalog.Update";
        map["CatalogActions.ChangeBaseUnit"] = "Catalog.Update";

        // Procurement iş kuralı eylemleri.
        map["ProcurementActions.Receive"] = "Procurement.Approve";

        // Finance iş kuralı eylemleri.
        map["FinanceActions.AllocatePayment"] = "Finance.Update";
        map["FinanceActions.AllocateCollection"] = "Finance.Update";
        map["FinanceActions.TimesheetCost"] = "Finance.Create";
        map["FinanceActions.ProgressPayment"] = "Finance.Create";
        map["FinanceActions.BudgetOverrun"] = "Budget.Read";

        // Üretilen per-entity API controller uç noktaları (134 tablo, IAM/Chat hariç):
        // Controller.Action → modül CRUD yetkisi. Başlangıçta otomatik etkinleştirilir.
        ModulesEndpointPermissionMap.Apply(map);

        // ER Overview iş akışlarından türetilen rapor uç noktaları:
        // Controller.Action → {Module}.{Report}.Read / .Export.
        ModulesReportEndpointPermissionMap.Apply(map);

        return map;
    }

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
