using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.BusinessPartners;

/// <summary>Müşteri, tedarikçi, taşeron ve diğer cari taraflar. Aynı kayıt birden çok rolde kullanılabilir.</summary>
public class BusinessPartner : AuditableEntity
{
    public PartnerType PartnerType { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
}
