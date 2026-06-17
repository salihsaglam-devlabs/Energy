namespace Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;

/// <summary>WorkOrderChecklistItem oluşturma isteği.</summary>
public class CreateWorkOrderChecklistItemRequest
{
    /// <summary>WorkOrderChecklistId</summary>
    public Guid WorkOrderChecklistId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>IsCompleted</summary>
    public bool IsCompleted { get; set; }
}
