using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Stok belge türü (giriş/çıkış/transfer/düzeltme).</summary>
public class StockDocumentType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>In, Out, Transfer, Adjustment.</summary>
    public string Direction { get; set; } = "In";
    public bool IsActive { get; set; } = true;
}
