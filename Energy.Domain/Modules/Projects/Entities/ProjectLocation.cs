using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>
/// Proje lokasyon hiyerarşisi
/// </summary>
public class ProjectLocation : AuditableEntity
{
    /// <summary>Proje</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Üst lokasyon</summary>
    public Guid? ParentLocationId { get; set; }

    /// <summary>Lokasyon kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Lokasyon adı</summary>
    public string Name { get; set; } = string.Empty;
}
