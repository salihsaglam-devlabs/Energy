namespace Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;

/// <summary>WarehouseTransfer liste satırı.</summary>
public class WarehouseTransferListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>SourceWarehouseId</summary>
    public Guid SourceWarehouseId { get; set; }

    /// <summary>TargetWarehouseId</summary>
    public Guid TargetWarehouseId { get; set; }

    /// <summary>TransferNo</summary>
    public string TransferNo { get; set; } = string.Empty;

    /// <summary>TransferDate</summary>
    public DateTime TransferDate { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
