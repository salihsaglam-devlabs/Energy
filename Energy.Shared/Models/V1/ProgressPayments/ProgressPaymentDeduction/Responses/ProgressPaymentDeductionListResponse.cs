namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;

/// <summary>ProgressPaymentDeduction liste satırı.</summary>
public class ProgressPaymentDeductionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ProgressPaymentId</summary>
    public Guid ProgressPaymentId { get; set; }

    /// <summary>DeductionType</summary>
    public string DeductionType { get; set; } = string.Empty;

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
