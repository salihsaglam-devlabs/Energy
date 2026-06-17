namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;

/// <summary>ProgressPaymentDeduction oluşturma isteği.</summary>
public class CreateProgressPaymentDeductionRequest
{
    /// <summary>ProgressPaymentId</summary>
    public Guid ProgressPaymentId { get; set; }

    /// <summary>DeductionType</summary>
    public string DeductionType { get; set; } = string.Empty;

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
