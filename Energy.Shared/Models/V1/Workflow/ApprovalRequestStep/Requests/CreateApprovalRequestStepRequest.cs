using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;

/// <summary>ApprovalRequestStep oluşturma isteği.</summary>
public class CreateApprovalRequestStepRequest
{
    /// <summary>Onay talebi</summary>
    public Guid ApprovalRequestId { get; set; }

    /// <summary>Kaynak adım</summary>
    public Guid ApprovalStepDefinitionId { get; set; }

    /// <summary>Sıra</summary>
    public int StepNo { get; set; }

    /// <summary>Adım durumu</summary>
    public ApprovalStepStatus Status { get; set; }

    /// <summary>ApprovalMode</summary>
    public ApprovalMode ApprovalMode { get; set; }

    /// <summary>RequiredApprovalCount</summary>
    public int? RequiredApprovalCount { get; set; }
}
