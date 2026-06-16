using Energy.Domain.Common;

namespace Energy.Domain.Modules.BusinessPartners;

/// <summary>Cari iletişim kişisi.</summary>
public class BusinessPartnerContact : AuditableEntity
{
    public Guid BusinessPartnerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsPrimary { get; set; }
}
