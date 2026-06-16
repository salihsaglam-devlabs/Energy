namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;

/// <summary>BusinessPartner detay görünümü.</summary>
public class BusinessPartnerDetailResponse
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

    /// <summary>Cari türü</summary>
    public string PartnerType { get; set; } = string.Empty;

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
