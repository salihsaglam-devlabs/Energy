using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

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
