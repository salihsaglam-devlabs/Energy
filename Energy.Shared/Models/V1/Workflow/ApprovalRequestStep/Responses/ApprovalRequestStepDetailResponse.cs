using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

/// <summary>ApprovalRequestStep detay görünümü.</summary>
public class ApprovalRequestStepDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

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
