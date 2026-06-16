using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>
/// Proje notları
/// </summary>
public class ProjectNote : AuditableEntity
{
    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Title</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Body</summary>
    public string? Body { get; set; }
}
