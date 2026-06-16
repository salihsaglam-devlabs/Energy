using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Workflow;

/// <summary>Adım bazlı onaycı (kullanıcı, rol veya departman).</summary>
public class ApprovalStepApprover : AuditableEntity
{
    public Guid ApprovalStepDefinitionId { get; set; }
    public ApproverType ApproverType { get; set; }
    public Guid? ApproverUserId { get; set; }
    public Guid? ApproverRoleId { get; set; }
    public Guid? ApproverDepartmentId { get; set; }
}
