using Energy.Domain.Common;

namespace Energy.Domain.Contracts;

/// <summary>Sözleşme tarafı.</summary>
public class ContractParty : AuditableEntity
{
    public Guid ContractId { get; set; }
    public Guid BusinessPartnerId { get; set; }
    public string PartyRole { get; set; } = string.Empty;
}
