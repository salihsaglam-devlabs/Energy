namespace Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;

/// <summary>ProgressPaymentSummary raporu satırı (salt-okunur projeksiyon).</summary>
public sealed class ProgressPaymentSummaryRowResponse
{
    /// <summary>Kaynak kayıt kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ProgressPaymentNo</summary>
    public string? ProgressPaymentNo { get; set; }

    /// <summary>ContractId</summary>
    public Guid ContractId { get; set; }

    /// <summary>GrossAmount</summary>
    public decimal GrossAmount { get; set; }

    /// <summary>NetAmount</summary>
    public decimal NetAmount { get; set; }

    /// <summary>PaymentPeriodStart</summary>
    public DateTime PaymentPeriodStart { get; set; }

    /// <summary>Status</summary>
    public string? Status { get; set; }
}
