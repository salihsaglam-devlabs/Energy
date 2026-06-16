namespace Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

/// <summary>EmployeeSkillAssignment liste satırı.</summary>
public class EmployeeSkillAssignmentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>EmployeeId</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>EmployeeSkillId</summary>
    public Guid EmployeeSkillId { get; set; }

    /// <summary>Level</summary>
    public int Level { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
