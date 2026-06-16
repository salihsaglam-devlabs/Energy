using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>
/// Metraj satırları
/// </summary>
public class MeasurementSheetLine : AuditableEntity
{
    /// <summary>MeasurementSheetId</summary>
    public Guid MeasurementSheetId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }
}
