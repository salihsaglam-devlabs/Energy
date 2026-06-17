namespace Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;

/// <summary>BusinessPartnerBankAccount güncelleme isteği.</summary>
public class UpdateBusinessPartnerBankAccountRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
