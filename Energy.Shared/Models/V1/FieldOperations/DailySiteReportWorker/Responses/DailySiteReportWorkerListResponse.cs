namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;

/// <summary>DailySiteReportWorker liste satırı.</summary>
public class DailySiteReportWorkerListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>DailySiteReports referansı</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>HoursWorked</summary>
    public decimal HoursWorked { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
