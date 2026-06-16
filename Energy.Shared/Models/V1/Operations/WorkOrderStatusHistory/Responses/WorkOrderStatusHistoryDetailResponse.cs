namespace Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;

/// <summary>WorkOrderStatusHistory detay görünümü.</summary>
public class WorkOrderStatusHistoryDetailResponse
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

    /// <summary>WorkOrderId</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>FromStatus</summary>
    public string FromStatus { get; set; } = string.Empty;

    /// <summary>ToStatus</summary>
    public string ToStatus { get; set; } = string.Empty;

    /// <summary>ChangedAt</summary>
    public DateTime ChangedAt { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
