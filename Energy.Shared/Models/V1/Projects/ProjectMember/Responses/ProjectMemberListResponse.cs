namespace Energy.Shared.Models.V1.Projects.ProjectMember.Responses;

/// <summary>ProjectMember liste satırı.</summary>
public class ProjectMemberListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Projects referansı</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? UserId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>ProjectRole</summary>
    public string? ProjectRole { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
