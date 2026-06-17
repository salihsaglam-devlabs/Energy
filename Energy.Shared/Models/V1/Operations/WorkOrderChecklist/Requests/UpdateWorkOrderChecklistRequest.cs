namespace Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;

/// <summary>WorkOrderChecklist güncelleme isteği.</summary>
public class UpdateWorkOrderChecklistRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrderId</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }
}
