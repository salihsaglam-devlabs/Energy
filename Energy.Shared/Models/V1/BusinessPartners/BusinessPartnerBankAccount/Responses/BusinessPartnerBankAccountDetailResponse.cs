namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

/// <summary>BusinessPartnerBankAccount detay görünümü.</summary>
public class BusinessPartnerBankAccountDetailResponse
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

    /// <summary>BankName</summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>Branch</summary>
    public string? Branch { get; set; }

    /// <summary>Iban</summary>
    public string Iban { get; set; } = string.Empty;

    /// <summary>CurrencyId</summary>
    public Guid? CurrencyId { get; set; }

    /// <summary>IsPrimary</summary>
    public bool IsPrimary { get; set; }
}
