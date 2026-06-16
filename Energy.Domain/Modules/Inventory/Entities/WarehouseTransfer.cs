using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Depolar arası transfer başlıkları
/// </summary>
public class WarehouseTransfer : AuditableEntity
{
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
}
