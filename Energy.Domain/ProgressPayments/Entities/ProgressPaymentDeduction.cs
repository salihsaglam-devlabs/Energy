using Energy.Domain.Common;

namespace Energy.Domain.ProgressPayments;

/// <summary>Hakediş kesintisi.</summary>
public class ProgressPaymentDeduction : AuditableEntity
{
    public Guid ProgressPaymentId { get; set; }
    public string DeductionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}
