namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;

/// <summary>ProgressPaymentDeduction güncelleme isteği.</summary>
public class UpdateProgressPaymentDeductionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ProgressPaymentId</summary>
    public Guid ProgressPaymentId { get; set; }

    /// <summary>DeductionType</summary>
    public string DeductionType { get; set; } = string.Empty;

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
