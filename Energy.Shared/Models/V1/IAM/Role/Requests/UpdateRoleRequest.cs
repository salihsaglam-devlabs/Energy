namespace Energy.Shared.Models.V1.IAM.Role.Requests;

/// <summary>Role güncelleme isteği.</summary>
public class UpdateRoleRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Rol adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Sistem rolü</summary>
    public bool IsSystem { get; set; }
}
