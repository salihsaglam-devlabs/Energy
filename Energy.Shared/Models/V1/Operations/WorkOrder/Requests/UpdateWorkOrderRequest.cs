using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Operations.WorkOrder.Requests;

/// <summary>WorkOrder güncelleme isteği.</summary>
public class UpdateWorkOrderRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
