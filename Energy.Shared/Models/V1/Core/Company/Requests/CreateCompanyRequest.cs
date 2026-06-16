namespace Energy.Shared.Models.V1.Core.Company.Requests;

/// <summary>Company oluşturma isteği.</summary>
public class CreateCompanyRequest
{
    /// <summary>Şirket kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Şirket adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Ana para birimi</summary>
    public Guid BaseCurrencyId { get; set; }

    /// <summary>TaxNumber</summary>
    public string? TaxNumber { get; set; }

    /// <summary>Address</summary>
    public string? Address { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
