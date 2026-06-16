using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;

/// <summary>Stok çıkış (issue) süreç isteği (FIFO, transaction-güvenli).</summary>
public sealed class StockIssueProcessRequest
{
    /// <summary>Çıkışın yapılacağı depo.</summary>
    [Required]
    public Guid WarehouseId { get; set; }

    /// <summary>Çıkışı yapılacak malzeme.</summary>
    [Required]
    public Guid MaterialId { get; set; }

    /// <summary>Ölçü birimi.</summary>
    [Required]
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>Çıkış miktarı (pozitif).</summary>
    [Range(0.000001, double.MaxValue)]
    public decimal Quantity { get; set; }

    /// <summary>İlişkili proje (opsiyonel).</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Açıklama (opsiyonel).</summary>
    public string? Note { get; set; }
}
