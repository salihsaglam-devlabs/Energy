namespace Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;

/// <summary>EmployeeSkill oluşturma isteği.</summary>
public class CreateEmployeeSkillRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
