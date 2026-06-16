using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>
/// Adımın gerçek onaycıları
/// </summary>
public class ApprovalRequestApprover : AuditableEntity
{
    /// <summary>Talep adımı</summary>
    public Guid ApprovalRequestStepId { get; set; }

    /// <summary>Gerçek onaycı</summary>
    public Guid UserId { get; set; }

    /// <summary>Kişisel onay durumu</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>İşlem zamanı</summary>
    public DateTime? ActionAt { get; set; }

    /// <summary>DelegatedFromUserId</summary>
    public Guid? DelegatedFromUserId { get; set; }
}
