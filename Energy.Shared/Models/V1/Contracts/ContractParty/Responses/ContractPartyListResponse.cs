namespace Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

/// <summary>ContractParty liste satırı.</summary>
public class ContractPartyListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Contracts referansı</summary>
    public Guid ContractId { get; set; }

    /// <summary>BusinessPartners referansı</summary>
    public Guid BusinessPartnerId { get; set; }

    /// <summary>PartyRole</summary>
    public string PartyRole { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
