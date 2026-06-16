namespace Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;

/// <summary>StockDocumentType güncelleme isteği.</summary>
public class UpdateStockDocumentTypeRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Direction</summary>
    public string Direction { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
