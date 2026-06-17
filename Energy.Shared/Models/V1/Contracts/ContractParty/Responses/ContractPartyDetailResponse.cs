namespace Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

/// <summary>ContractParty detay görünümü.</summary>
public class ContractPartyDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Contracts referansı</summary>
    public Guid ContractId { get; set; }

    /// <summary>BusinessPartners referansı</summary>
    public Guid BusinessPartnerId { get; set; }

    /// <summary>PartyRole</summary>
    public string PartyRole { get; set; } = string.Empty;
}
