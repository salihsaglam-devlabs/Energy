namespace Energy.Shared.Models.V1.Projects.ProjectMember.Requests;

/// <summary>ProjectMember güncelleme isteği.</summary>
public class UpdateProjectMemberRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Projects referansı</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? UserId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>ProjectRole</summary>
    public string? ProjectRole { get; set; }
}
