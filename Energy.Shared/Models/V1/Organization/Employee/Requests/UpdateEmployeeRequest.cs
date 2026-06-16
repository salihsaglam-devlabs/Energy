namespace Energy.Shared.Models.V1.Organization.Employee.Requests;

/// <summary>Employee güncelleme isteği.</summary>
public class UpdateEmployeeRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
