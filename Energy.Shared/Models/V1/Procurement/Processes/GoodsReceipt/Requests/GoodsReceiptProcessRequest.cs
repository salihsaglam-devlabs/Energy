using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;

/// <summary>Mal kabul (goods receipt) süreç isteği (irsaliye -> stok girişi).</summary>
public sealed class GoodsReceiptProcessRequest
{
    /// <summary>Stok girişine dönüştürülecek satınalma irsaliyesi kimliği.</summary>
    [Required]
    public Guid PurchaseReceiptId { get; set; }
}
