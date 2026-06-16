using Energy.Domain.Common;

namespace Energy.Domain.Modules.Contracts;

/// <summary>
/// Ek protokoller
/// </summary>
public class ContractAmendment : AuditableEntity
{
    /// <summary>ContractId</summary>
    public Guid ContractId { get; set; }

    /// <summary>AmendmentNo</summary>
    public string AmendmentNo { get; set; } = string.Empty;

    /// <summary>AmendmentDate</summary>
    public DateTime AmendmentDate { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>AmountDelta</summary>
    public decimal AmountDelta { get; set; }
}
