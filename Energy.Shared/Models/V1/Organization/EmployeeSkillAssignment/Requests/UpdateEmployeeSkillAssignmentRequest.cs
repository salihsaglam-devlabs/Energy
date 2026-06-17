namespace Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;

/// <summary>EmployeeSkillAssignment güncelleme isteği.</summary>
public class UpdateEmployeeSkillAssignmentRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>EmployeeId</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>EmployeeSkillId</summary>
    public Guid EmployeeSkillId { get; set; }

    /// <summary>Level</summary>
    public int Level { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
