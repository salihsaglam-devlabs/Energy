namespace Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;

/// <summary>WorkOrderChecklist oluşturma isteği.</summary>
public class CreateWorkOrderChecklistRequest
{
    /// <summary>WorkOrderId</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }
}
