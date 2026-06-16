using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>
/// Günlük saha malzemeleri
/// </summary>
public class DailySiteReportMaterial : AuditableEntity
{
    /// <summary>DailySiteReports referansı</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }
}
