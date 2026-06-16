using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Çıkış satırı ile lot maliyet dağılımı
/// </summary>
public class StockIssueAllocation : AuditableEntity
{
    /// <summary>Çıkış satırı</summary>
    public Guid StockDocumentLineId { get; set; }

    /// <summary>Lot</summary>
    public Guid StockLotId { get; set; }

    /// <summary>Dağıtılan miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Maliyet</summary>
    public decimal UnitCost { get; set; }
}
