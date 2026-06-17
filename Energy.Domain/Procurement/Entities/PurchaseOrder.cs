using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Procurement;

/// <summary>Satın alma sipariş başlığı.</summary>
public class PurchaseOrder : AuditableEntity
{
    public Guid SupplierId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid CurrencyId { get; set; }
    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
    public string OrderNo { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public Guid? ApprovalRequestId { get; set; }
}
