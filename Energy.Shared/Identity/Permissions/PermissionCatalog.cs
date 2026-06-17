namespace Energy.Shared.Identity.Permissions;

/// <summary>
/// Uygulamanın desteklediği her <c>Modül.Eylem</c> yetkisinin derleme zamanı listesi.
/// Bu, başlangıçta <c>permissions</c> tablosuna yansıtılan tek doğruluk kaynağıdır.
/// Arayüz burada satır oluşturmaz veya silmez; yeni bir yetki eklemek sürüm
/// zamanına ait bir değişikliktir.
/// </summary>
public static class PermissionCatalog
{
    /// <summary>Gösterge panosunu okuma yetkisi.</summary>
    public const string DashboardRead = "Dashboard.Read";

    /// <summary>Tek bir kullanıcıyı okuma yetkisi.</summary>
    public const string UserRead = "User.Read";
    /// <summary>Tüm kullanıcıları okuma yetkisi.</summary>
    public const string UserReadAll = "User.ReadAll";
    /// <summary>Kullanıcı oluşturma yetkisi.</summary>
    public const string UserCreate = "User.Create";
    /// <summary>Kullanıcı güncelleme yetkisi.</summary>
    public const string UserUpdate = "User.Update";
    /// <summary>Kullanıcı silme yetkisi.</summary>
    public const string UserDelete = "User.Delete";

    /// <summary>Tek bir rolü okuma yetkisi.</summary>
    public const string RoleRead = "Role.Read";
    /// <summary>Tüm rolleri okuma yetkisi.</summary>
    public const string RoleReadAll = "Role.ReadAll";
    /// <summary>Rol oluşturma yetkisi.</summary>
    public const string RoleCreate = "Role.Create";
    /// <summary>Rol güncelleme yetkisi.</summary>
    public const string RoleUpdate = "Role.Update";
    /// <summary>Rol silme yetkisi.</summary>
    public const string RoleDelete = "Role.Delete";

    /// <summary>Tek bir yetkiyi okuma yetkisi.</summary>
    public const string PermissionRead = "Permission.Read";
    /// <summary>Tüm yetkileri okuma yetkisi.</summary>
    public const string PermissionReadAll = "Permission.ReadAll";

    /// <summary>Tek bir menüyü okuma yetkisi.</summary>
    public const string MenuRead = "Menu.Read";
    /// <summary>Tüm menüleri okuma yetkisi.</summary>
    public const string MenuReadAll = "Menu.ReadAll";
    /// <summary>Menü oluşturma yetkisi.</summary>
    public const string MenuCreate = "Menu.Create";
    /// <summary>Menü güncelleme yetkisi.</summary>
    public const string MenuUpdate = "Menu.Update";
    /// <summary>Menü silme yetkisi.</summary>
    public const string MenuDelete = "Menu.Delete";

    /// <summary>Tek bir API erişim kaydını okuma yetkisi.</summary>
    public const string ApiAccessRead = "ApiAccess.Read";
    /// <summary>Tüm API erişim kayıtlarını okuma yetkisi.</summary>
    public const string ApiAccessReadAll = "ApiAccess.ReadAll";
    /// <summary>API erişim kaydı oluşturma yetkisi.</summary>
    public const string ApiAccessCreate = "ApiAccess.Create";
    /// <summary>API erişim kaydı güncelleme yetkisi.</summary>
    public const string ApiAccessUpdate = "ApiAccess.Update";
    /// <summary>API erişim kaydı silme yetkisi.</summary>
    public const string ApiAccessDelete = "ApiAccess.Delete";

    /// <summary>Tek bir yerelleştirme girdisini okuma yetkisi.</summary>
    public const string LocalizationRead = "Localization.Read";
    /// <summary>Tüm yerelleştirme girdilerini okuma yetkisi.</summary>
    public const string LocalizationReadAll = "Localization.ReadAll";
    /// <summary>Yerelleştirme girdisi oluşturma yetkisi.</summary>
    public const string LocalizationCreate = "Localization.Create";
    /// <summary>Yerelleştirme girdisi güncelleme yetkisi.</summary>
    public const string LocalizationUpdate = "Localization.Update";
    /// <summary>Yerelleştirme girdisi silme yetkisi.</summary>
    public const string LocalizationDelete = "Localization.Delete";

    /// <summary>Tek bir denetim günlüğünü okuma yetkisi.</summary>
    public const string LogRead = "Log.Read";
    /// <summary>Tüm denetim günlüklerini okuma yetkisi.</summary>
    public const string LogReadAll = "Log.ReadAll";

    /// <summary>Ayarları okuma yetkisi.</summary>
    public const string SettingRead = "Setting.Read";
    /// <summary>Ayarları güncelleme yetkisi.</summary>
    public const string SettingUpdate = "Setting.Update";

    /// <summary>
    /// Sistem bakımı: (idempotent) veri tohumlayıcılarını isteğe bağlı tetikleme.
    /// Yüksek ayrıcalıklı işlem; SystemAdmin'e verilir, SuperAdmin tarafından atlanır.
    /// </summary>
    public const string SystemSeed = "System.Seed";

    /// <summary>Self servis: her kimlik doğrulanmış kullanıcı kendi profilini okuyabilir (varsayılan verilir).</summary>
    public const string ProfileRead = "Profile.Read";
    /// <summary>Self servis: her kimlik doğrulanmış kullanıcı kendi profilini güncelleyebilir (varsayılan verilir).</summary>
    public const string ProfileUpdate = "Profile.Update";

    /// <summary>
    /// İşbirliği: her kimlik doğrulanmış kullanıcı sohbeti kullanabilir. Varsayılan
    /// verilir; böylece tüm roller kutudan çıktığı gibi birbiriyle mesajlaşabilir.
    /// </summary>
    public const string ChatUse = "Chat.Use";

    /// <summary>Self servis: kullanıcı kendi tercihlerini okuyabilir (bildirim sesi, tema vb.). Varsayılan verilir.</summary>
    public const string UserSettingsRead = "UserSettings.Read";
    /// <summary>Self servis: kullanıcı kendi tercihlerini güncelleyebilir. Varsayılan verilir.</summary>
    public const string UserSettingsUpdate = "UserSettings.Update";

    /// <summary>
    /// CRUD yetkisi (Read/ReadAll/Create/Update/Delete) üretilen kurumsal iş modülleri.
    /// Tasarım dokümanındaki 22 modülün iş modüllerini kapsar.
    /// </summary>
    public static IReadOnlyList<string> CrudModules { get; } =
    [
        "Core",
        "Organization",
        "BusinessPartners",
        "Projects",
        "Catalog",
        "Inventory",
        "Requests",
        "Procurement",
        "Operations",
        "FieldOperations",
        "HR",
        "Assets",
        "Finance",
        "Budget",
        "Contracts",
        "ProgressPayments",
        "Documents",
        "Workflow",
        "Notifications",
        "Reporting",
    ];

    /// <summary>Modül CRUD dışındaki özel iş yetkileri.</summary>
    public static IReadOnlyList<string> SpecialPermissions { get; } =
    [
        "Default.Read", "Default.ReadAll", "Default.Create", "Default.Update", "Default.Delete",
        "Inventory.Approve", "Inventory.Transfer", "Inventory.Count", "Inventory.Reverse",
        "Procurement.Approve",
        "Workflow.Approve", "Workflow.Reject", "Workflow.Return",
        "Documents.Upload", "Documents.Download", "Documents.Version",
        "Reporting.Export",
        "Chat.GroupManage", "Chat.GroupDelete", "Chat.MemberAdd", "Chat.MemberRemove", "Chat.AdminAssign",
    ];

    /// <summary>
    /// ER Overview iş akışlarından türetilen rapor yetkileri (her rapor için
    /// <c>{Module}.{Report}.Read</c> ve ayrı <c>{Module}.{Report}.Export</c>).
    /// Bu liste rapor generator'ı (tools/scaffold/generate_reports.py) ile birebir
    /// hizalıdır; yeni rapor eklemek sürüm zamanına ait bir değişikliktir.
    /// </summary>
    public static IReadOnlyList<string> ReportPermissions { get; } =
    [
        "Procurement.PurchaseOrderSummary.Read", "Procurement.PurchaseOrderSummary.Export",
        "Inventory.StockBalanceReport.Read", "Inventory.StockBalanceReport.Export",
        "Projects.ProjectStatusReport.Read", "Projects.ProjectStatusReport.Export",
        "HR.TimesheetSummary.Read", "HR.TimesheetSummary.Export",
        "Finance.PayableAging.Read", "Finance.PayableAging.Export",
        "Finance.ReceivableAging.Read", "Finance.ReceivableAging.Export",
        "ProgressPayments.ProgressPaymentSummary.Read", "ProgressPayments.ProgressPaymentSummary.Export",
    ];

    /// <summary>Tanımlanmış her yetki kodunun düz listesi.</summary>
    public static IReadOnlyList<PermissionDescriptor> All { get; } = BuildAll();

    private static IReadOnlyList<PermissionDescriptor> BuildAll()
    {
        var list = new List<PermissionDescriptor>
        {
            Describe(DashboardRead),

            Describe(UserRead), Describe(UserReadAll), Describe(UserCreate), Describe(UserUpdate), Describe(UserDelete),

            Describe(RoleRead), Describe(RoleReadAll), Describe(RoleCreate), Describe(RoleUpdate), Describe(RoleDelete),

            Describe(PermissionRead), Describe(PermissionReadAll),

            Describe(MenuRead), Describe(MenuReadAll), Describe(MenuCreate), Describe(MenuUpdate), Describe(MenuDelete),

            Describe(ApiAccessRead), Describe(ApiAccessReadAll), Describe(ApiAccessCreate), Describe(ApiAccessUpdate), Describe(ApiAccessDelete),

            Describe(LocalizationRead), Describe(LocalizationReadAll), Describe(LocalizationCreate), Describe(LocalizationUpdate), Describe(LocalizationDelete),

            Describe(LogRead), Describe(LogReadAll),

            Describe(SettingRead), Describe(SettingUpdate),

            Describe(SystemSeed),

            Describe(ProfileRead), Describe(ProfileUpdate),

            Describe(ChatUse),

            Describe(UserSettingsRead), Describe(UserSettingsUpdate),
        };

        foreach (var module in CrudModules)
        {
            list.Add(Describe($"{module}.{PermissionActions.Read}"));
            list.Add(Describe($"{module}.{PermissionActions.ReadAll}"));
            list.Add(Describe($"{module}.{PermissionActions.Create}"));
            list.Add(Describe($"{module}.{PermissionActions.Update}"));
            list.Add(Describe($"{module}.{PermissionActions.Delete}"));
        }

        foreach (var code in SpecialPermissions)
        {
            list.Add(Describe(code));
        }

        foreach (var code in ReportPermissions)
        {
            list.Add(Describe(code));
        }

        // Aynı kod birden çok kez gelirse (örn. Inventory CRUD + özel) tekilleştir.
        return list
            .GroupBy(item => item.Code, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .ToList();
    }

    /// <summary>Üyelik kontrolleri için pratik küme (O(1)).</summary>
    public static IReadOnlySet<string> AllCodes { get; } =
        new HashSet<string>(All.Select(item => item.Code), StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Her kimlik doğrulanmış kullanıcının roründen bağımsız olarak sahip olması
    /// gereken yetkiler — açık atama gerektirmeyen "taban". Gösterge panosu ve
    /// self servis profilin her zaman erişilebilir olması için her role tohumlanır.
    /// </summary>
    public static IReadOnlyList<string> DefaultGrants { get; } =
    [
        DashboardRead,
        ProfileRead,
        ProfileUpdate,
        ChatUse,
        UserSettingsRead,
        UserSettingsUpdate,
    ];

    /// <summary>Görünen ad için yerelleştirme anahtarı.</summary>
    public static string BuildDisplayNameKey(string code) => $"Permissions.{code}.Name";

    /// <summary>Açıklama için yerelleştirme anahtarı.</summary>
    public static string BuildDescriptionKey(string code) => $"Permissions.{code}.Description";

    /// <summary>Bir yetki kodunu ayrıştırıp tanımlayıcı (descriptor) oluşturur.</summary>
    private static PermissionDescriptor Describe(string code)
    {
        var parts = code.Split('.', 2);
        if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
        {
            throw new InvalidOperationException($"Permission code '{code}' does not match the required 'Module.Action' format.");
        }

        return new PermissionDescriptor(
            Code: code,
            Module: parts[0],
            Action: parts[1],
            DisplayNameKey: BuildDisplayNameKey(code),
            DescriptionKey: BuildDescriptionKey(code));
    }
}

/// <summary>Bir yetkinin kodunu, modülünü, eylemini ve yerelleştirme anahtarlarını taşıyan tanımlayıcı.</summary>
public readonly record struct PermissionDescriptor(
    string Code,
    string Module,
    string Action,
    string DisplayNameKey,
    string DescriptionKey);

