namespace Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;

/// <summary>ApprovalStepApprover güncelleme isteği.</summary>
public class UpdateApprovalStepApproverRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
