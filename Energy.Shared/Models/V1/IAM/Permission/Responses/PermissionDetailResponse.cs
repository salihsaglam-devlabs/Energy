namespace Energy.Shared.Models.V1.IAM.Permission.Responses;

/// <summary>Permission detay görünümü.</summary>
public class PermissionDetailResponse
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

    /// <summary>Permission kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Modül</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>İşlem</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Görünen ad anahtarı</summary>
    public string DisplayNameKey { get; set; } = string.Empty;
}
