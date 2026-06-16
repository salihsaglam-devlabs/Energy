using Energy.Domain.Common;

namespace Energy.Domain.Projects;

/// <summary>Proje notu.</summary>
public class ProjectNote : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
}
