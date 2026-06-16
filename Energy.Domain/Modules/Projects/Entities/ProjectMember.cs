using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

/// <summary>
/// Proje kullanıcı ve personel atamaları
/// </summary>
public class ProjectMember : AuditableEntity
{
    /// <summary>Projects referansı</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? UserId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>ProjectRole</summary>
    public string? ProjectRole { get; set; }
}
