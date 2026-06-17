namespace Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;

/// <summary>EmployeeSkill güncelleme isteği.</summary>
public class UpdateEmployeeSkillRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
