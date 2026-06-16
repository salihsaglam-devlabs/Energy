namespace Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

/// <summary>ProjectPhas liste satırı.</summary>
public class ProjectPhasListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Proje</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Üst faz</summary>
    public Guid? ParentPhaseId { get; set; }

    /// <summary>Faz kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Faz adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>İlerleme yüzdesi</summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
