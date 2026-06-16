namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;

/// <summary>BusinessPartnerAddress liste satırı.</summary>
public class BusinessPartnerAddressListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
