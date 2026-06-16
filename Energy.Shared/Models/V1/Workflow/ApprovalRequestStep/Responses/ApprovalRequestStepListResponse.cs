namespace Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

/// <summary>ApprovalRequestStep liste satırı.</summary>
public class ApprovalRequestStepListResponse
{
    /// <summary>Kimlik.</summary>
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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
