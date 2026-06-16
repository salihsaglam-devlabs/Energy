namespace Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;

/// <summary>TimesheetSummary raporu satırı (salt-okunur projeksiyon).</summary>
public sealed class TimesheetSummaryRowResponse
{
    /// <summary>Kaynak kayıt kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>TimesheetNo</summary>
    public string? TimesheetNo { get; set; }

    /// <summary>PeriodStart</summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>PeriodEnd</summary>
    public DateTime PeriodEnd { get; set; }

    /// <summary>Status</summary>
    public string? Status { get; set; }
}
