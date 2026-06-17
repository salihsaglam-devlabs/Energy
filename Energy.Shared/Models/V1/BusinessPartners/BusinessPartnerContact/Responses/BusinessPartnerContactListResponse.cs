namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

/// <summary>BusinessPartnerContact liste satırı.</summary>
public class BusinessPartnerContactListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>BusinessPartnerId</summary>
    public Guid BusinessPartnerId { get; set; }

    /// <summary>FullName</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Title</summary>
    public string? Title { get; set; }

    /// <summary>Phone</summary>
    public string? Phone { get; set; }

    /// <summary>Email</summary>
    public string? Email { get; set; }

    /// <summary>IsPrimary</summary>
    public bool IsPrimary { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
