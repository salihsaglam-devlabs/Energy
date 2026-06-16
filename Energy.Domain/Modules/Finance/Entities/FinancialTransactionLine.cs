using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>Ön muhasebe hareket satırı.</summary>
public class FinancialTransactionLine : AuditableEntity
{
    public Guid FinancialTransactionId { get; set; }
    public Guid? CostCenterId { get; set; }
    public Guid? ProjectId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
