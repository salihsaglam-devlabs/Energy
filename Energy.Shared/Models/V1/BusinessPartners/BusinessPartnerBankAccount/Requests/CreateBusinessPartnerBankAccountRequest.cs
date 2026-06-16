namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;

/// <summary>BusinessPartnerBankAccount oluşturma isteği.</summary>
public class CreateBusinessPartnerBankAccountRequest
{
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
