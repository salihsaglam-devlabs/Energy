using Energy.Domain.Common;

namespace Energy.Domain.Modules.Contracts;

/// <summary>
/// Sözleşme tarafları
/// </summary>
public class ContractParty : AuditableEntity
{
    /// <summary>Contracts referansı</summary>
    public Guid ContractId { get; set; }

    /// <summary>BusinessPartners referansı</summary>
    public Guid BusinessPartnerId { get; set; }

    /// <summary>PartyRole</summary>
    public string PartyRole { get; set; } = string.Empty;
}
