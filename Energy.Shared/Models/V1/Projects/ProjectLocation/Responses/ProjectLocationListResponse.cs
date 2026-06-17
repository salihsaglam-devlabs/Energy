namespace Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;

/// <summary>ProjectLocation liste satırı.</summary>
public class ProjectLocationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Proje</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Üst lokasyon</summary>
    public Guid? ParentLocationId { get; set; }

    /// <summary>Lokasyon kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Lokasyon adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
