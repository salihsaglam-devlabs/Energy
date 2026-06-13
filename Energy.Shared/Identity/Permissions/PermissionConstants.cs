namespace Energy.Shared.Identity.Permissions;

/// <summary>
/// Her <c>Modül.Eylem</c> yetki kodunda kullanılan standart eylem fiilleri.
/// </summary>
public static class PermissionActions
{
    /// <summary>Tek bir kaydı okuma eylemi.</summary>
    public const string Read = "Read";
    /// <summary>Tüm kayıtları okuma eylemi.</summary>
    public const string ReadAll = "ReadAll";
    /// <summary>Kayıt oluşturma eylemi.</summary>
    public const string Create = "Create";
    /// <summary>Kayıt güncelleme eylemi.</summary>
    public const string Update = "Update";
    /// <summary>Kayıt silme eylemi.</summary>
    public const string Delete = "Delete";
}

/// <summary>
/// Standart modül adları. Yeni modüller aynı PascalCase biçimini izlemelidir.
/// </summary>
public static class PermissionModules
{
    /// <summary>Gösterge panosu modülü.</summary>
    public const string Dashboard = "Dashboard";
    /// <summary>Kullanıcı modülü.</summary>
    public const string User = "User";
    /// <summary>Rol modülü.</summary>
    public const string Role = "Role";
    /// <summary>Yetki modülü.</summary>
    public const string Permission = "Permission";
    /// <summary>Menü modülü.</summary>
    public const string Menu = "Menu";
    /// <summary>API erişim modülü.</summary>
    public const string ApiAccess = "ApiAccess";
    /// <summary>Yerelleştirme modülü.</summary>
    public const string Localization = "Localization";
    /// <summary>Günlük (log) modülü.</summary>
    public const string Log = "Log";
    /// <summary>Ayar modülü.</summary>
    public const string Setting = "Setting";
    /// <summary>Profil modülü.</summary>
    public const string Profile = "Profile";
    /// <summary>Kullanıcı tercihleri modülü.</summary>
    public const string UserSettings = "UserSettings";
}
