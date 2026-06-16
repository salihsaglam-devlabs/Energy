using Energy.Domain.Common;

namespace Energy.Domain.Finance;

/// <summary>Finans hesabı (kasa, banka vb.).</summary>
public class FinancialAccount : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Cash, Bank, Other.</summary>
    public string AccountType { get; set; } = "Bank";
    public Guid? CurrencyId { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Maliyet merkezi.</summary>
public class CostCenter : AuditableEntity
{
    public Guid? ParentCostCenterId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>Ön muhasebe hareket başlığı (immutable davranır).</summary>
public class FinancialTransaction : AuditableEntity
{
    public FinancialTransactionType TransactionType { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid? FinancialAccountId { get; set; }
    public Guid? CostCenterId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? RelatedModule { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? Description { get; set; }
    public bool IsReversed { get; set; }
}

/// <summary>Ön muhasebe hareket satırı.</summary>
public class FinancialTransactionLine : AuditableEntity
{
    public Guid FinancialTransactionId { get; set; }
    public Guid? CostCenterId { get; set; }
    public Guid? ProjectId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

/// <summary>Borç kaydı.</summary>
public class Payable : AuditableEntity
{
    public Guid PartnerId { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal Amount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime DueDate { get; set; }
    public string? RelatedModule { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public bool IsClosed { get; set; }
}

/// <summary>Alacak kaydı.</summary>
public class Receivable : AuditableEntity
{
    public Guid PartnerId { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal Amount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime DueDate { get; set; }
    public string? RelatedModule { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public bool IsClosed { get; set; }
}

/// <summary>Ödeme başlığı.</summary>
public class Payment : AuditableEntity
{
    public Guid PartnerId { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid? FinancialAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentNo { get; set; } = string.Empty;
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}

/// <summary>Ödemenin borçlara dağılımı.</summary>
public class PaymentAllocation : AuditableEntity
{
    public Guid PaymentId { get; set; }
    public Guid PayableId { get; set; }
    public decimal Amount { get; set; }
}

/// <summary>Tahsilat başlığı.</summary>
public class Collection : AuditableEntity
{
    public Guid PartnerId { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid? FinancialAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CollectionDate { get; set; }
    public string CollectionNo { get; set; } = string.Empty;
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}

/// <summary>Tahsilatın alacaklara dağılımı.</summary>
public class CollectionAllocation : AuditableEntity
{
    public Guid CollectionId { get; set; }
    public Guid ReceivableId { get; set; }
    public decimal Amount { get; set; }
}

