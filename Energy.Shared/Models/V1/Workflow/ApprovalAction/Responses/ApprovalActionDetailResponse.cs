using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;

/// <summary>ApprovalAction detay görünümü.</summary>
public class ApprovalActionDetailResponse
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

    /// <summary>Opsiyonel adım</summary>
    public Guid? ApprovalRequestStepId { get; set; }

    /// <summary>İşlem yapan</summary>
    public Guid UserId { get; set; }

    /// <summary>Approve, Reject, Return, Cancel</summary>
    public ApprovalActionType ActionType { get; set; }

    /// <summary>İşlem zamanı</summary>
    public DateTime ActionAt { get; set; }

    /// <summary>Açıklama</summary>
    public string? Note { get; set; }
}
