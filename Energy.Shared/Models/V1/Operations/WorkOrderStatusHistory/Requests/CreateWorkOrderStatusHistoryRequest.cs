namespace Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;

/// <summary>WorkOrderStatusHistory oluşturma isteği.</summary>
public class CreateWorkOrderStatusHistoryRequest
{
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
