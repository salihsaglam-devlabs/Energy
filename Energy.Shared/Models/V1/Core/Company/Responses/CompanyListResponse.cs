namespace Energy.Shared.Models.V1.Core.Company.Responses;

/// <summary>Company liste satırı.</summary>
public class CompanyListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
