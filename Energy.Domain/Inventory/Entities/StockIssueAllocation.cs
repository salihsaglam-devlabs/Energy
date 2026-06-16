using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Çıkış satırının lotlara FIFO dağılımı.</summary>
public class StockIssueAllocation : AuditableEntity
{
    public Guid StockDocumentLineId { get; set; }
    public Guid StockLotId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
}
