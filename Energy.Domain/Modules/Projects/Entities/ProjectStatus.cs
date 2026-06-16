using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>
/// Proje durumları
/// </summary>
public class ProjectStatus : AuditableEntity
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }

    /// <summary>IsClosedState</summary>
    public bool IsClosedState { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
