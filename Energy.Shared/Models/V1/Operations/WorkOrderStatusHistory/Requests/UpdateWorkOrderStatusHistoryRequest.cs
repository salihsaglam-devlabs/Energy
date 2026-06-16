namespace Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;

/// <summary>WorkOrderStatusHistory güncelleme isteği.</summary>
public class UpdateWorkOrderStatusHistoryRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
