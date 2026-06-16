using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>Personel masraf satırı.</summary>
public class ExpenseClaimLine : AuditableEntity
{
    public Guid ExpenseClaimId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public decimal Amount { get; set; }
    public string? Category { get; set; }
}
