using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>
/// Proje fazları ve WBS benzeri kırılım
/// </summary>
public class ProjectPhas : AuditableEntity
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
