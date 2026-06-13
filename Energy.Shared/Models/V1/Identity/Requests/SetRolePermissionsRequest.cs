namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>Bir rolün sahip olduğu yetki (permission) kümesini tümüyle değiştiren istek.</summary>
public sealed class SetRolePermissionsRequest
{
    /// <summary>Role atanacak yetki kodlarının tam listesi.</summary>
    public IReadOnlyCollection<string> PermissionCodes { get; set; } = Array.Empty<string>();
}
