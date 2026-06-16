using Energy.Domain.Common;

namespace Energy.Domain.BusinessPartners;

/// <summary>Cari banka hesabı.</summary>
public class BusinessPartnerBankAccount : AuditableEntity
{
    public Guid BusinessPartnerId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string? Branch { get; set; }
    public string Iban { get; set; } = string.Empty;
    public Guid? CurrencyId { get; set; }
    public bool IsPrimary { get; set; }
}
