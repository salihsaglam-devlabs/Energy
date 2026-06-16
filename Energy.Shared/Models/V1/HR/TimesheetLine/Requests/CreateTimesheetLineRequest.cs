namespace Energy.Shared.Models.V1.HR.TimesheetLine.Requests;

/// <summary>TimesheetLine oluşturma isteği.</summary>
public class CreateTimesheetLineRequest
{
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
}
