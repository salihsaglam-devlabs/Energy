using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.ProgressPayments;

/// <summary>Hakediş başlığı (sözleşmeye bağlı).</summary>
public class ProgressPayment : AuditableEntity
{
    public Guid ContractId { get; set; }
    public Guid? PartnerId { get; set; }
    public string ProgressPaymentNo { get; set; } = string.Empty;
    public DateTime PaymentPeriodStart { get; set; }
    public DateTime PaymentPeriodEnd { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal DeductionTotal { get; set; }
    public decimal NetAmount { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}
