namespace Energy.Shared.Models.V1.HR.TimesheetLine.Responses;

/// <summary>TimesheetLine liste satırı.</summary>
public class TimesheetLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Timesheets referansı</summary>
    public Guid TimesheetId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>Projects referansı</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>WorkOrderId</summary>
    public Guid? WorkOrderId { get; set; }

    /// <summary>WorkDate</summary>
    public DateTime WorkDate { get; set; }

    /// <summary>NormalHours</summary>
    public decimal NormalHours { get; set; }

    /// <summary>OvertimeHours</summary>
    public decimal OvertimeHours { get; set; }

    /// <summary>HourlyCost</summary>
    public decimal HourlyCost { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
