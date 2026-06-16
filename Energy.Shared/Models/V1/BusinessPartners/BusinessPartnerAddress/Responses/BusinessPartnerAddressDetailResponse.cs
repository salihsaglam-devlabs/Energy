namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;

/// <summary>BusinessPartnerAddress detay görünümü.</summary>
public class BusinessPartnerAddressDetailResponse
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

    /// <summary>BusinessPartnerId</summary>
    public Guid BusinessPartnerId { get; set; }

    /// <summary>AddressType</summary>
    public string AddressType { get; set; } = string.Empty;

    /// <summary>AddressLine</summary>
    public string AddressLine { get; set; } = string.Empty;

    /// <summary>City</summary>
    public string? City { get; set; }

    /// <summary>Country</summary>
    public string? Country { get; set; }

    /// <summary>PostalCode</summary>
    public string? PostalCode { get; set; }

    /// <summary>IsPrimary</summary>
    public bool IsPrimary { get; set; }
}
