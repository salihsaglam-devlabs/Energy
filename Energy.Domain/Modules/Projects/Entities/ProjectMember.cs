using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>Proje üyesi (kullanıcı veya personel ataması).</summary>
public class ProjectMember : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? ProjectRole { get; set; }
}
