using Energy.Domain.Common;

namespace Energy.Domain.Contracts;

/// <summary>Ek protokol.</summary>
public class ContractAmendment : AuditableEntity
{
    public Guid ContractId { get; set; }
    public string AmendmentNo { get; set; } = string.Empty;
    public DateTime AmendmentDate { get; set; }
    public string? Description { get; set; }
    public decimal AmountDelta { get; set; }
}
