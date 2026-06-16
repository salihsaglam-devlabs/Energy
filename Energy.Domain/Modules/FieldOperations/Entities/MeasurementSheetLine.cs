using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>Metraj satırı.</summary>
public class MeasurementSheetLine : AuditableEntity
{
    public Guid MeasurementSheetId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
