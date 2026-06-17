namespace Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;

/// <summary>PurchaseOrderLine güncelleme isteği.</summary>
public class UpdatePurchaseOrderLineRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Sipariş</summary>
    public Guid PurchaseOrderId { get; set; }

    /// <summary>Talep satırı</summary>
    public Guid? RequestLineId { get; set; }

    /// <summary>Malzeme</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Fiyat</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Para birimi</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>ReceivedQuantity</summary>
    public decimal ReceivedQuantity { get; set; }
}
