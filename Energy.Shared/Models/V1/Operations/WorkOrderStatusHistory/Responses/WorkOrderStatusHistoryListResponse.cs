using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;

/// <summary>WorkOrderStatusHistory liste satırı.</summary>
public class WorkOrderStatusHistoryListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrderId</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>FromStatus</summary>
    public WorkOrderStatus FromStatus { get; set; }

    /// <summary>ToStatus</summary>
    public WorkOrderStatus ToStatus { get; set; }

    /// <summary>ChangedAt</summary>
    public DateTime ChangedAt { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
