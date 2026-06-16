using Energy.Domain.Common;

namespace Energy.Domain.ProgressPayments;

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

/// <summary>Hakediş satırı.</summary>
public class ProgressPaymentLine : AuditableEntity
{
    public Guid ProgressPaymentId { get; set; }
    public Guid? ContractLineId { get; set; }
    public Guid? MeasurementSheetLineId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
}

/// <summary>Hakediş kesintisi.</summary>
public class ProgressPaymentDeduction : AuditableEntity
{
    public Guid ProgressPaymentId { get; set; }
    public string DeductionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}

