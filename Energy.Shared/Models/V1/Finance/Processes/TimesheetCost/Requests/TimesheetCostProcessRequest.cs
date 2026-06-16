using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;

/// <summary>Puantaj maliyet süreç isteği (HR Cost akışı).</summary>
public sealed class TimesheetCostProcessRequest
{
    /// <summary>Maliyetlendirilecek puantaj (Timesheet) kimliği.</summary>
    [Required]
    public Guid TimesheetId { get; set; }

    /// <summary>Maliyet hareketinin para birimi.</summary>
    [Required]
    public Guid CurrencyId { get; set; }
}
