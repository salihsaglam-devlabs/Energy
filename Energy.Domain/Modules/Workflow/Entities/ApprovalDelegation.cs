using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>
/// Geçici onay yetkisi devri
/// </summary>
public class ApprovalDelegation : AuditableEntity
{
    /// <summary>DelegatorUserId</summary>
    public Guid DelegatorUserId { get; set; }

    /// <summary>DelegateUserId</summary>
    public Guid DelegateUserId { get; set; }

    /// <summary>StartDate</summary>
    public DateTime StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime EndDate { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
