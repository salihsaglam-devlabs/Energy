namespace Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;

/// <summary>ApprovalRequestStep güncelleme isteği.</summary>
public class UpdateApprovalRequestStepRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
