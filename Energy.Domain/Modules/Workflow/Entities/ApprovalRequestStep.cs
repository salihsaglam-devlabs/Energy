using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>
/// Talebe bağlı adım örnekleri
/// </summary>
public class ApprovalRequestStep : AuditableEntity
{
    /// <summary>Onay talebi</summary>
    public Guid ApprovalRequestId { get; set; }

    /// <summary>Kaynak adım</summary>
    public Guid ApprovalStepDefinitionId { get; set; }

    /// <summary>Sıra</summary>
    public int StepNo { get; set; }

    /// <summary>Adım durumu</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>ApprovalMode</summary>
    public string ApprovalMode { get; set; } = string.Empty;

    /// <summary>RequiredApprovalCount</summary>
    public int? RequiredApprovalCount { get; set; }
}
