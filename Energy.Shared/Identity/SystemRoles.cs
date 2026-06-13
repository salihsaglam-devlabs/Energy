namespace Energy.Shared.Identity;

/// <summary>
/// Yerleşik roller için kararlı tanımlayıcılar. SuperAdmin rolü her zaman vardır
/// ve her yetki kontrolünü atlar; yeniden adlandırılamaz veya silinemez.
/// </summary>
public static class SystemRoles
{
    /// <summary>Her yetkiyi atlayan, silinemez ve yeniden adlandırılamaz süper yönetici rolü.</summary>
    public const string SuperAdmin = "SuperAdmin";
}
