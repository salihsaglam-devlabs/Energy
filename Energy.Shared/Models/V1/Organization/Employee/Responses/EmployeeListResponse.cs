namespace Energy.Shared.Models.V1.Organization.Employee.Responses;

/// <summary>Employee liste satırı.</summary>
public class EmployeeListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>CompanyId</summary>
    public Guid CompanyId { get; set; }

    /// <summary>BranchId</summary>
    public Guid? BranchId { get; set; }

    /// <summary>DepartmentId</summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>EmployeePositionId</summary>
    public Guid? EmployeePositionId { get; set; }

    /// <summary>UserId</summary>
    public Guid? UserId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>FirstName</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>LastName</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>NationalId</summary>
    public string? NationalId { get; set; }

    /// <summary>Phone</summary>
    public string? Phone { get; set; }

    /// <summary>Email</summary>
    public string? Email { get; set; }

    /// <summary>HireDate</summary>
    public DateTime? HireDate { get; set; }

    /// <summary>TerminationDate</summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
