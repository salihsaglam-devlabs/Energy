using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>
/// Personel masraf satırları
/// </summary>
public class ExpenseClaimLine : AuditableEntity
{
    /// <summary>ExpenseClaimId</summary>
    public Guid ExpenseClaimId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>ExpenseDate</summary>
    public DateTime ExpenseDate { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Category</summary>
    public string? Category { get; set; }
}
