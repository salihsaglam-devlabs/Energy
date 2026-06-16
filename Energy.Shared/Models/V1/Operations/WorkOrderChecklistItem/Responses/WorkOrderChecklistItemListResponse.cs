namespace Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;

/// <summary>WorkOrderChecklistItem liste satırı.</summary>
public class WorkOrderChecklistItemListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrderChecklistId</summary>
    public Guid WorkOrderChecklistId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>IsCompleted</summary>
    public bool IsCompleted { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
