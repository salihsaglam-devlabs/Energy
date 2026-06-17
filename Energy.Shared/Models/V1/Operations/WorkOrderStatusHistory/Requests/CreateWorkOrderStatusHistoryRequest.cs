using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;

/// <summary>WorkOrderStatusHistory oluşturma isteği.</summary>
public class CreateWorkOrderStatusHistoryRequest
{
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
}
