namespace Energy.Shared.Models.V1.IAM.Role.Responses;

/// <summary>Role liste satırı.</summary>
public class RoleListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Rol adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Sistem rolü</summary>
    public bool IsSystem { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
