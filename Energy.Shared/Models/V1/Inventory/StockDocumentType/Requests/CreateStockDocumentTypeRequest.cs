namespace Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;

/// <summary>StockDocumentType oluşturma isteği.</summary>
public class CreateStockDocumentTypeRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Direction</summary>
    public string Direction { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
