namespace Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;

/// <summary>ProjectLocation güncelleme isteği.</summary>
public class UpdateProjectLocationRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Proje</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Üst lokasyon</summary>
    public Guid? ParentLocationId { get; set; }

    /// <summary>Lokasyon kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Lokasyon adı</summary>
    public string Name { get; set; } = string.Empty;
}
