using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;

/// <summary>BusinessPartner güncelleme isteği.</summary>
public class UpdateBusinessPartnerRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Cari türü</summary>
    public PartnerType PartnerType { get; set; }

    /// <summary>Cari kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Cari adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Vergi numarası</summary>
    public string? TaxNumber { get; set; }

    /// <summary>TaxOffice</summary>
    public string? TaxOffice { get; set; }

    /// <summary>Phone</summary>
    public string? Phone { get; set; }

    /// <summary>Email</summary>
    public string? Email { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
