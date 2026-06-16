using Energy.Domain.Common;

namespace Energy.Domain.Modules.BusinessPartners;

/// <summary>
/// Cari banka hesapları
/// </summary>
public class BusinessPartnerBankAccount : AuditableEntity
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
