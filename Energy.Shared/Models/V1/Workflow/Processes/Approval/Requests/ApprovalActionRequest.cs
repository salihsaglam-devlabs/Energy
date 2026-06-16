namespace Energy.Shared.Models.V1.Workflow.Processes.Approval.Requests;

/// <summary>Onay eylemi (onayla/ret/iptal) isteği. Açıklama/not taşır.</summary>
public sealed class ApprovalActionRequest
{
    /// <summary>Eyleme eşlik eden açıklama/not (opsiyonel).</summary>
    public string? Note { get; set; }
}
