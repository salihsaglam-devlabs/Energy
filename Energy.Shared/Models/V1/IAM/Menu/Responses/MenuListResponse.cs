namespace Energy.Shared.Models.V1.IAM.Menu.Responses;

/// <summary>Menu liste satırı.</summary>
public class MenuListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Üst menü</summary>
    public Guid? ParentId { get; set; }

    /// <summary>Lokalizasyon anahtarı</summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>URL</summary>
    public string? Url { get; set; }

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
