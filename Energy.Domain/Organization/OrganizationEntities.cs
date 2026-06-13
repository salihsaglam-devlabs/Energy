using Energy.Domain.Common;

namespace Energy.Domain.Organization;

/// <summary>Pozisyon tanımı (master/lookup).</summary>
public class EmployeePosition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>Yetkinlik tanımı.</summary>
public class EmployeeSkill : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

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

/// <summary>Personel ↔ yetkinlik N:N bağlantısı.</summary>
public class EmployeeSkillAssignment : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Guid EmployeeSkillId { get; set; }
    public int Level { get; set; }
    public string? Note { get; set; }
}

/// <summary>İzin talebi. Workflow onayına bağlanabilir.</summary>
public class LeaveRequest : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Days { get; set; }
    public string? Reason { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}

/// <summary>Personel masraf talebi başlığı.</summary>
public class ExpenseClaim : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid CurrencyId { get; set; }
    public string ClaimNo { get; set; } = string.Empty;
    public DateTime ClaimDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}

/// <summary>Personel masraf satırı.</summary>
public class ExpenseClaimLine : AuditableEntity
{
    public Guid ExpenseClaimId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public decimal Amount { get; set; }
    public string? Category { get; set; }
}

