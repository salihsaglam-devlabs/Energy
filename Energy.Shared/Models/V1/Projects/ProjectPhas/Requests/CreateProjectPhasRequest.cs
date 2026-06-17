namespace Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;

/// <summary>ProjectPhas oluşturma isteği.</summary>
public class CreateProjectPhasRequest
{
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
}
