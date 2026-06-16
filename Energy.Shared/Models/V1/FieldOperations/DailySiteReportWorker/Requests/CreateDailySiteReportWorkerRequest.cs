namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;

/// <summary>DailySiteReportWorker oluşturma isteği.</summary>
public class CreateDailySiteReportWorkerRequest
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
