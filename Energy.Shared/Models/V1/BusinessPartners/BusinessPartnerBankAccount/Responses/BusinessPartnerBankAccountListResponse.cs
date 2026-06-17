namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

/// <summary>BusinessPartnerBankAccount liste satırı.</summary>
public class BusinessPartnerBankAccountListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
