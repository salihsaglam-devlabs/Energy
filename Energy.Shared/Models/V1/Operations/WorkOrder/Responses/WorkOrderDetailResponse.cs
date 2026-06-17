using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

/// <summary>WorkOrder detay görünümü.</summary>
public class WorkOrderDetailResponse
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

    /// <summary>İş emri türü</summary>
    public Guid WorkOrderTypeId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Opsiyonel faz</summary>
    public Guid? ProjectPhaseId { get; set; }

    /// <summary>Opsiyonel lokasyon</summary>
    public Guid? ProjectLocationId { get; set; }

    /// <summary>Durum</summary>
    public WorkOrderStatus Status { get; set; }

    /// <summary>İş emri no</summary>
    public string WorkOrderNo { get; set; } = string.Empty;

    /// <summary>Başlık</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>PlannedStart</summary>
    public DateTime? PlannedStart { get; set; }

    /// <summary>PlannedEnd</summary>
    public DateTime? PlannedEnd { get; set; }
}
