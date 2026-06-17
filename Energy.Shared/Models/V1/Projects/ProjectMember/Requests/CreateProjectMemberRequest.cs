namespace Energy.Shared.Models.V1.Projects.ProjectMember.Requests;

/// <summary>ProjectMember oluşturma isteği.</summary>
public class CreateProjectMemberRequest
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
