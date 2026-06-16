using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>
/// Adım bazlı kişi, rol veya grup onaycıları
/// </summary>
public class ApprovalStepApprover : AuditableEntity
{
    /// <summary>Adım</summary>
    public Guid ApprovalStepDefinitionId { get; set; }

    /// <summary>User, Role, ProjectRole, DepartmentManager</summary>
    public string ApproverType { get; set; } = string.Empty;

    /// <summary>Kişi bazlı onaycı</summary>
    public Guid? ApproverUserId { get; set; }

    /// <summary>Rol bazlı onaycı</summary>
    public Guid? ApproverRoleId { get; set; }

    /// <summary>Departman bazlı onaycı</summary>
    public Guid? ApproverDepartmentId { get; set; }
}
