using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>Personel kartı. Kullanıcı hesabından bağımsızdır, opsiyonel olarak bağlanır.</summary>
public class Employee : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? EmployeePositionId { get; set; }
    public Guid? UserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public bool IsActive { get; set; } = true;
}
