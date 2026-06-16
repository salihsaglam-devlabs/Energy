using Energy.Domain.Common;

namespace Energy.Domain.FieldOperations;

/// <summary>Günlük saha malzemesi.</summary>
public class DailySiteReportMaterial : AuditableEntity
{
    public Guid DailySiteReportId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
}
