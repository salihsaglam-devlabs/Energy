using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;

/// <summary>PurchaseOrder oluşturma isteği.</summary>
public class CreatePurchaseOrderRequest
{
    /// <summary>Tedarikçi</summary>
    public Guid SupplierId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Durum</summary>
    public PurchaseOrderStatus Status { get; set; }

    /// <summary>Sipariş no</summary>
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>OrderDate</summary>
    public DateTime OrderDate { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
