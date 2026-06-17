namespace Energy.Shared.Models.V1.IAM.Role.Responses;

/// <summary>Role detay görünümü.</summary>
public class RoleDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Rol adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Sistem rolü</summary>
    public bool IsSystem { get; set; }
}
