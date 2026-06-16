using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>
/// Ön muhasebe hareket satırları
/// </summary>
public class FinancialTransactionLine : AuditableEntity
{
    /// <summary>FinancialTransactionId</summary>
    public Guid FinancialTransactionId { get; set; }

    /// <summary>CostCenterId</summary>
    public Guid? CostCenterId { get; set; }

    /// <summary>ProjectId</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }
}
