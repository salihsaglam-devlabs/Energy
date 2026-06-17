namespace Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

/// <summary>ProjectPhas detay görünümü.</summary>
public class ProjectPhasDetailResponse
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
