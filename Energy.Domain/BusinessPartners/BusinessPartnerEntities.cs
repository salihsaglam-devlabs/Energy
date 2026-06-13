using Energy.Domain.Common;

namespace Energy.Domain.BusinessPartners;

/// <summary>Müşteri, tedarikçi, taşeron ve diğer cari taraflar. Aynı kayıt birden çok rolde kullanılabilir.</summary>
public class BusinessPartner : AuditableEntity
{
    public PartnerType PartnerType { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Cari iletişim kişisi.</summary>
public class BusinessPartnerContact : AuditableEntity
{
    public Guid BusinessPartnerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsPrimary { get; set; }
}

/// <summary>Cari adresi.</summary>
public class BusinessPartnerAddress : AuditableEntity
{
    public Guid BusinessPartnerId { get; set; }
    public string AddressType { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public bool IsPrimary { get; set; }
}

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

