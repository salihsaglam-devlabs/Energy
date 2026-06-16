using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;

/// <summary>WarehouseTransfer güncelleme isteği.</summary>
public class UpdateWarehouseTransferRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
    public DocumentStatus Status { get; set; }
}
