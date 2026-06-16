namespace Energy.Shared.Models.V1.IAM.Permission.Responses;

/// <summary>Permission liste satırı.</summary>
public class PermissionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Permission kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Modül</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>İşlem</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Görünen ad anahtarı</summary>
    public string DisplayNameKey { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
