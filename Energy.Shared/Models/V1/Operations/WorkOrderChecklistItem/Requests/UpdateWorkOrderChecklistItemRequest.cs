namespace Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;

/// <summary>WorkOrderChecklistItem güncelleme isteği.</summary>
public class UpdateWorkOrderChecklistItemRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrderChecklistId</summary>
    public Guid WorkOrderChecklistId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>IsCompleted</summary>
    public bool IsCompleted { get; set; }
}
