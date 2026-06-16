namespace Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

/// <summary>WorkOrder liste satırı.</summary>
public class WorkOrderListResponse
{
    /// <summary>Kimlik.</summary>
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
    public string Status { get; set; } = string.Empty;

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
