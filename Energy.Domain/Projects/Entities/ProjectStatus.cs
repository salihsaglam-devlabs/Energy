using Energy.Domain.Common;

namespace Energy.Domain.Projects;

/// <summary>Proje durumu (lookup/master).</summary>
public class ProjectStatus : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsClosedState { get; set; }
    public bool IsActive { get; set; } = true;
}
