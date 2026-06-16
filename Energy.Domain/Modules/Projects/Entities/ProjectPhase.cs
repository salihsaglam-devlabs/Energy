using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>Proje fazı / WBS benzeri kırılım.</summary>
public class ProjectPhase : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ParentPhaseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal ProgressPercentage { get; set; }
    public DateTime? PlannedStart { get; set; }
    public DateTime? PlannedEnd { get; set; }
}
