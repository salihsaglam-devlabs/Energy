namespace Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;

/// <summary>PurchaseOrder liste satırı.</summary>
public class PurchaseOrderListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Tedarikçi</summary>
    public Guid SupplierId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Durum</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Sipariş no</summary>
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>OrderDate</summary>
    public DateTime OrderDate { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
