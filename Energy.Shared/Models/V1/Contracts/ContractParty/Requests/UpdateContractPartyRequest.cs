namespace Energy.Shared.Models.V1.Contracts.ContractParty.Requests;

/// <summary>ContractParty güncelleme isteği.</summary>
public class UpdateContractPartyRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Contracts referansı</summary>
    public Guid ContractId { get; set; }

    /// <summary>BusinessPartners referansı</summary>
    public Guid BusinessPartnerId { get; set; }

    /// <summary>PartyRole</summary>
    public string PartyRole { get; set; } = string.Empty;
}
