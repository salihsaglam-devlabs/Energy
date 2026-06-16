using Energy.Domain.Common;

namespace Energy.Domain.Modules.ProgressPayments;

/// <summary>
/// Hakediş kesintileri
/// </summary>
public class ProgressPaymentDeduction : AuditableEntity
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
