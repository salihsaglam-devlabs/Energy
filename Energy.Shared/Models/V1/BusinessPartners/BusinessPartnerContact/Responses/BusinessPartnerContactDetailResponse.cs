namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

/// <summary>BusinessPartnerContact detay görünümü.</summary>
public class BusinessPartnerContactDetailResponse
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
