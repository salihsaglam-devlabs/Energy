namespace Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

/// <summary>ApprovalRequest detay görünümü.</summary>
public class ApprovalRequestDetailResponse
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

    /// <summary>Akış versiyonu</summary>
    public Guid ApprovalDefinitionVersionId { get; set; }

    /// <summary>Kaynak modül</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>Kaynak nesne türü</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>Kaynak nesne</summary>
    public Guid RelatedEntityId { get; set; }

    /// <summary>Talep eden</summary>
    public Guid RequestedByUserId { get; set; }

    /// <summary>Durum</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>CurrentStepNo</summary>
    public int CurrentStepNo { get; set; }
}
