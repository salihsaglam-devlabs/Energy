using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>Proje türü (lookup/master).</summary>
public class ProjectType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
