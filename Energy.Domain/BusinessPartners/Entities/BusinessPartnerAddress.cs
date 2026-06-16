using Energy.Domain.Common;

namespace Energy.Domain.BusinessPartners;

/// <summary>Cari adresi.</summary>
public class BusinessPartnerAddress : AuditableEntity
{
    public Guid BusinessPartnerId { get; set; }
    public string AddressType { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public bool IsPrimary { get; set; }
}
