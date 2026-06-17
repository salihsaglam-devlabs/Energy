namespace Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses;

/// <summary>Puantaj maliyet sürecinin sonucu: üretilen finansal hareket kimliği.</summary>
public sealed class TimesheetCostProcessResponse
{
    /// <summary>Üretilen finansal maliyet hareketinin kimliği.</summary>
    public Guid FinancialTransactionId { get; set; }
}
