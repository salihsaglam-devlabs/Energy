using Energy.Domain.Common;

namespace Energy.Domain.Modules.BusinessPartners;

/// <summary>
/// Cari iletişim kişileri
/// </summary>
public class BusinessPartnerContact : AuditableEntity
{
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
}
