using Energy.Domain.Common;

namespace Energy.Domain.FieldOperations;

/// <summary>Günlük saha personeli.</summary>
public class DailySiteReportWorker : AuditableEntity
{
    public Guid DailySiteReportId { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal HoursWorked { get; set; }
    public string? Note { get; set; }
}
