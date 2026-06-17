using Energy.Domain.Common;

namespace Energy.Domain.Projects;

/// <summary>Proje lokasyon hiyerarşisi.</summary>
public class ProjectLocation : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ParentLocationId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
