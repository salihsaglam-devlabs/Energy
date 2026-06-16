using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>
/// Proje türleri
/// </summary>
public class ProjectType : AuditableEntity
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
