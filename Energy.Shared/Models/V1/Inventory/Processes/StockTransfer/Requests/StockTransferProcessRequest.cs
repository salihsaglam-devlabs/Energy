using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;

/// <summary>Depolar arası stok transfer süreç isteği (transaction-güvenli).</summary>
public sealed class StockTransferProcessRequest
{
    /// <summary>Kaynak depo.</summary>
    [Required]
    public Guid SourceWarehouseId { get; set; }

    /// <summary>Hedef depo.</summary>
    [Required]
    public Guid TargetWarehouseId { get; set; }

    /// <summary>Transfer edilecek malzeme.</summary>
    [Required]
    public Guid MaterialId { get; set; }

    /// <summary>Ölçü birimi.</summary>
    [Required]
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>Transfer miktarı (pozitif).</summary>
    [Range(0.000001, double.MaxValue)]
    public decimal Quantity { get; set; }

    /// <summary>Açıklama (opsiyonel).</summary>
    public string? Note { get; set; }
}
