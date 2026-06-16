using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

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
