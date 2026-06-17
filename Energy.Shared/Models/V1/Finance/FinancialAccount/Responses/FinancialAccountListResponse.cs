namespace Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;

/// <summary>FinancialAccount liste satırı.</summary>
public class FinancialAccountListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>AccountType</summary>
    public string AccountType { get; set; } = string.Empty;

    /// <summary>CurrencyId</summary>
    public Guid? CurrencyId { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
