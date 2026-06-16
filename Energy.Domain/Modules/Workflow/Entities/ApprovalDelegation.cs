using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>Geçici onay yetkisi devri.</summary>
public class ApprovalDelegation : AuditableEntity
{
    public Guid DelegatorUserId { get; set; }
    public Guid DelegateUserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}
