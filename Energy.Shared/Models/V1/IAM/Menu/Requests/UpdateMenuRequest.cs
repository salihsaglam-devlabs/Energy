namespace Energy.Shared.Models.V1.IAM.Menu.Requests;

/// <summary>Menu güncelleme isteği.</summary>
public class UpdateMenuRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Üst menü</summary>
    public Guid? ParentId { get; set; }

    /// <summary>Lokalizasyon anahtarı</summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>URL</summary>
    public string? Url { get; set; }

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }
}
