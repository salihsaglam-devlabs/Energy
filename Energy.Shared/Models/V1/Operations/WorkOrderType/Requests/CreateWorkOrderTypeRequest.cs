namespace Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;

/// <summary>WorkOrderType oluşturma isteği.</summary>
public class CreateWorkOrderTypeRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
