using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

/// <summary>ApprovalRequestApprover detay görünümü.</summary>
public class ApprovalRequestApproverDetailResponse
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

    /// <summary>Talep adımı</summary>
    public Guid ApprovalRequestStepId { get; set; }

    /// <summary>Gerçek onaycı</summary>
    public Guid UserId { get; set; }

    /// <summary>Kişisel onay durumu</summary>
    public ApprovalApproverStatus Status { get; set; }

    /// <summary>İşlem zamanı</summary>
    public DateTime? ActionAt { get; set; }

    /// <summary>DelegatedFromUserId</summary>
    public Guid? DelegatedFromUserId { get; set; }
}
