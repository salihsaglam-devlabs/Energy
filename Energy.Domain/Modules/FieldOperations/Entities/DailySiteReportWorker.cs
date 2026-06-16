using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>
/// Günlük saha personelleri
/// </summary>
public class DailySiteReportWorker : AuditableEntity
{
    /// <summary>DailySiteReports referansı</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>HoursWorked</summary>
    public decimal HoursWorked { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
